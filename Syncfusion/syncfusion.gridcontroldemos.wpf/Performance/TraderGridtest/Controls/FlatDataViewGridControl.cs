using Syncfusion.Windows.Collections;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    public class FlatDataViewGridControl : GridControlBase
    {
        public FlatDataViewGridControl()
        {
            RowHeights.DefaultLineSize = 20;
            ColumnWidths.DefaultLineSize = 60;
            ColumnWidths[0] = 45;
            FooterRows = 1;
            _increaseBrush = new SolidColorBrush(Color.FromArgb(92, 0, 255, 0));
            _reducedBrush = new SolidColorBrush(Color.FromArgb(92, 255, 0, 0));
            _increaseBrush.Freeze();
            _reducedBrush.Freeze();
        }
        PropertyDescriptorCollection _pdc;
        Brush _increaseBrush, _reducedBrush;
        int _lastAddNewIndex = -1;
        public static readonly DependencyProperty SourceListProperty =
           DependencyProperty.Register("SourceList", typeof(IBindingList), typeof(FlatDataViewGridControl), new PropertyMetadata(null));

        public IBindingList SourceList
        {
            get { return (IBindingList)GetValue(SourceListProperty); }
            set
            {
                if (!object.ReferenceEquals(SourceList, value))
                {
                    if (SourceList != null)
                    {
                        UnwireSourceList();
                    }
                    SetValue(SourceListProperty, value);
                    if (SourceList != null)
                    {
                        WireSourceList();
                    }
                    InitializeTotals();
                    OnSourceListChanged();
                }

            }
        }



        public int BlinkTime
        {
            get { return (int)GetValue(BlinkTimeProperty); }
            set { SetValue(BlinkTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlinkTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlinkTimeProperty =
            DependencyProperty.Register("BlinkTime", typeof(int), typeof(FlatDataViewGridControl), new PropertyMetadata(null));



        List<double> _totals = new List<double>();
        bool _invalidateWholeSummaryRow = false;

        public List<double> Totals
        {
            get { return _totals; }
        }

        protected override void OnPrepareRenderCell(GridPrepareRenderCellEventArgs e)
        {
            base.OnPrepareRenderCell(e);
            if (e.Handled) return;

            if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 1)
            {
                BlinkState bs = GetBlinkState(e.Cell.RowIndex - 1, e.Cell.ColumnIndex - 1);
                switch (bs)
                {
                    case BlinkState.Increased:
                        e.Style.Background = _increaseBrush;
                        break;
                    case BlinkState.Reduced:
                        e.Style.Background = _reducedBrush;
                        break;
                }
            }
        }
        protected override void WireModel()
        {
            Model.QueryCellInfo += new GridQueryCellInfoEventHandler(Model_QueryCellInfo);
            Model.CommitCellInfo += new GridCommitCellInfoEventHandler(Model_CommitCellInfo);
            base.WireModel();
        }
        protected override void UnwireModel()
        {
            Model.QueryCellInfo -= new GridQueryCellInfoEventHandler(Model_QueryCellInfo);
            Model.CommitCellInfo -= new GridCommitCellInfoEventHandler(Model_CommitCellInfo);
            base.UnwireModel();
        }
        private void Model_CommitCellInfo(object sender, GridCommitCellInfoEventArgs e)
        {
            if (e.Handled) return;
            if (_pdc == null)
                return;

            if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 1)
            {
                if (e.Style.Store.IsValueModified(GridStyleInfoStore.CellValueProperty)
                    || e.Sip == GridStyleInfoStore.CellValueProperty)
                {
                    PropertyDescriptor pd = _pdc[e.Cell.ColumnIndex - 1];
                    object item = SourceList[e.Cell.RowIndex - 1];
                    object value = e.Style.CellValue;
                    pd.SetValue(item, value);
                }
            }
        }



        private void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (e.Handled) return;
            if (_pdc == null)
                return;
            if (e.Cell.ColumnIndex > 0)
            {
                PropertyDescriptor pd;
                pd = _pdc[e.Cell.ColumnIndex - 1];
                if (e.Cell.RowIndex == 0)
                {
                    e.Style.CellValue = pd.DisplayName;
                }
                else if (e.Cell.RowIndex - 1 < SourceList.Count)
                {
                    object item = SourceList[e.Cell.RowIndex - 1];
                    object value = pd.GetValue(item);
                    e.Style.CellValueType = pd.PropertyType;
                    e.Style.CellValue = value;
                }
                else if (e.Cell.RowIndex - 1 == SourceList.Count)
                {
                    if (pd.PropertyType == typeof(double))
                    {
                        e.Style.CellValueType = typeof(double);
                        e.Style.CellValue = Totals[e.Cell.ColumnIndex - 1];
                    }
                }
            }
            else
            {
                if (e.Cell.RowIndex - 1 == SourceList.Count)
                {
                    e.Style.CellValue = "Totals";
                }
                else if (e.Cell.RowIndex > 0)
                {
                    // if I do this (row numbers) then I need to add also extra code
                    // when inserting/removing rows so that these row header cells get refreshed.
                    //e.Style.CellValue = e.Cell.RowIndex - 1;
                }
            }
        }

        private void InitializeTotals()
        {
            _totals.Clear();
            for (int n = 0; n < _pdc.Count; n++)
                _totals.Add(0);

            if (SourceList == null)
                return;

            for (int r = 0; r < SourceList.Count; r++)
            {
                object item = SourceList[r];
                for (int n = 0; n < _pdc.Count; n++)
                {
                    PropertyDescriptor pd = _pdc[n];
                    if (pd.PropertyType == typeof(double))
                        _totals[n] += Convert.ToDouble(pd.GetValue(item));
                }
            }
        }

        protected virtual void OnSourceListChanged()
        {
        }
        DataTable _dt;
        bool _dataSourceRaisesTwoItemAddedEvents;
        private void WireSourceList()
        {
            System.Windows.Interop.ComponentDispatcher.ThreadIdle += new EventHandler(ComponentDispatcher_ThreadIdle);
            SourceList.ListChanged += new ListChangedEventHandler(SourceList_ListChanged);
            Model.RowCount = SourceList.Count + 1 + FooterRows;
            _pdc = ListUtil.GetItemProperties(SourceList);
            Model.ColumnCount = _pdc.Count + 1;
            _dt = GetDataTable(this.SourceList);
            WireDataTable(_dt);
        }

        private void WireDataTable(DataTable dt)
        {
            if (dt == null) return;
            dt.ColumnChanging += new DataColumnChangeEventHandler(Dt_ColumnChanging);
            dt.RowDeleting += new DataRowChangeEventHandler(Dt_RowDeleting);
        }

        private void Dt_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
            string name = e.Column.ColumnName;
            ChangedFieldInfo ci = new ChangedFieldInfo(_pdc, name, e.Row[e.Column], e.ProposedValue);
            AddChangedField(ci);
        }

        private void Dt_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            PrepareRemoving(e.Row);
        }

        private void PrepareRemoving(DataRow row)
        {
            OnPrepareRemoving(row);
        }

        protected virtual void OnPrepareRemoving(DataRow row)
        {
            for (int n = 0; n < _pdc.Count; n++)
            {
                PropertyDescriptor pd = _pdc[n];
                if (pd.PropertyType == typeof(double))
                {
                    object value = GetValue(row, pd);
                    ChangedFieldInfo ci = new ChangedFieldInfo(_pdc, pd.Name, value, null);
                    this.AddChangedField(ci);
                }
            }
        }

        private static object GetValue(DataRow row, PropertyDescriptor pd)
        {
            if (row is DataRow)
            {
                // PropertyDescriptor is for the DataRowView - but here
                // we only have access to the DataRow.
                return ((DataRow)row)[pd.Name];
            }
            else
            {
                return pd.GetValue(row);
            }
        }

        DataTable GetDataTable(object datasource)
        {
            if (datasource is DataTable)
                return (DataTable)datasource;

            else if (datasource is DataView)
                return ((DataView)datasource).Table;

            return null;
        }
        private void SourceList_ListChanged(object? sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemChanged:
                    if (_lastAddNewIndex == -1 || _lastAddNewIndex != e.NewIndex)
                    {
                        OnSourceListItemChanged(e);
                    }
                    _lastAddNewIndex = -1;
                    break;
                case ListChangedType.ItemAdded:
                    this._lastAddNewIndex = -1;
                    OnSourceListItemAdded(e);
                    this._lastAddNewIndex = e.NewIndex;
                    break;
                case ListChangedType.ItemDeleted:
                    this._lastAddNewIndex = -1;
                    OnSourceListItemDeleted(e);
                    break;
                case ListChangedType.ItemMoved:
                    if (_lastAddNewIndex != e.NewIndex)
                        OnSourceListItemMoved(e);
                    _lastAddNewIndex = -1;
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    OnSourceListPropertyDescriptorAdded(e);
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    OnSourceListPropertyDescriptorDeleted(e);
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    OnSourceListPropertyDescriptorChanged(e);
                    break;
                case ListChangedType.Reset:
                    OnSourceListReset(e);
                    InitializeTotals();
                    break;
            }
            ClearChangedFields();
        }



        private void OnSourceListItemChanged(ListChangedEventArgs e)
        {
            int rowIndex = e.NewIndex + 1;
            bool rowVisible = IsRowVisible(rowIndex);
            bool anyCellVisible = false;
            ChangedFieldInfoCollection cc = GetChangedFields();
            if (cc.Count > 0)
            {
                foreach (ChangedFieldInfo fi in cc)
                {
                    int columnIndex = fi.FieldIndex + 1;

                    PropertyDescriptor pd = _pdc[columnIndex - 1];
                    if (pd.PropertyType == typeof(double))
                    {
                        _totals[columnIndex - 1] += fi.Delta;
                        if (!_invalidateWholeSummaryRow)
                            InvalidateCell(new RowColumnIndex(SourceList.Count + 1, columnIndex));
                    }

                    RowColumnIndex cellRowColumnIndex = new RowColumnIndex(rowIndex, columnIndex);
                    if (CurrentCell.CellRowColumnIndex == cellRowColumnIndex)
                    {
                        if (!CurrentCell.IsInConfirmChanges)
                        {
                            CurrentCell.RejectChanges();
                            Model.VolatileCellStyles.Clear(cellRowColumnIndex);
                            RenderStyles.Clear(cellRowColumnIndex);
                            CurrentCell.Renderer.RefreshContent();
                        }
                        else
                        {
                            this.InvalidateCell(cellRowColumnIndex);
                            continue;
                        }
                    }
                    else
                        InvalidateCell(cellRowColumnIndex);
                    if (rowVisible)
                        anyCellVisible |= IsColumnVisible(columnIndex);
                }
                if (_invalidateWholeSummaryRow)
                    InvalidateCell(GridRangeInfo.Row(SourceList.Count + 1));
            }
            else
            {
                for (int n = 0; n < _pdc.Count; n++)
                {
                    int columnIndex = n + 1;
                    RowColumnIndex cell = new RowColumnIndex(rowIndex, columnIndex);
                    if (rowVisible)
                        anyCellVisible |= IsColumnVisible(columnIndex);
                }

            }
            Blink(e);
            ProcessBlinkQueue();
            if (anyCellVisible)
                InvalidateVisual(false);
        }
        private void OnSourceListItemAdded(ListChangedEventArgs e)
        {
            int rowIndex = e.NewIndex + 1;
            BlinkQueueInsertRecord(e.NewIndex, null);
            Model.InsertRows(rowIndex, 1);
            PrepareItemAdded(SourceList[e.NewIndex]);
            Blink(e);
            object item = SourceList[e.NewIndex];
            for (int n = 0; n < _pdc.Count; n++)
            {
                int columnIndex = n + 1;
                PropertyDescriptor pd = _pdc[n];
                if (pd.PropertyType == typeof(double))
                {
                    object value = pd.GetValue(item);
                    if (value != null && !(value is DBNull))
                    {
                        _totals[n] += Convert.ToDouble(value);
                        if (!_invalidateWholeSummaryRow)
                            InvalidateCell(new RowColumnIndex(SourceList.Count + 1, columnIndex));
                    }
                }
            }
            if (_invalidateWholeSummaryRow)
                InvalidateCell(GridRangeInfo.Row(SourceList.Count + 1));
            if (rowIndex < TopRowIndex)
                VScrollBar.Value += RowHeights[rowIndex];

            InvalidateVisual(true);
        }

        private void OnSourceListItemDeleted(ListChangedEventArgs e)
        {
            int rowIndex = e.NewIndex + 1;
            if (rowIndex < TopRowIndex)
                VScrollBar.Value -= RowHeights[rowIndex];
            Model.RemoveRows(rowIndex, 1);
            BlinkQueueDeleteRecord(e.NewIndex, null);
        }


        private void OnSourceListItemMoved(ListChangedEventArgs e)
        {
            //Console.WriteLine("OnSourceListItemMoved {0}->{1} ({2})", e.OldIndex, e.NewIndex, this.dt.DefaultView[e.NewIndex][0]);
            Model.MoveRows(e.OldIndex + 1, 1, e.NewIndex + 1);

            Dictionary<RowColumnIndex, bool> moveDirtyRows = new Dictionary<RowColumnIndex, bool>();
            //RemoveDirtyCellRows(e.OldIndex + 1, 1, moveDirtyRows);
            //InsertDirtyCellRows(e.NewIndex + 1, 1, moveDirtyRows);

            BlinkTable moveBlinks = new BlinkTable();
            BlinkQueueDeleteRecord(e.OldIndex, moveBlinks);
            BlinkQueueInsertRecord(e.NewIndex, moveBlinks);
            Blink(e);

            InvalidateVisual(true);

            OnSourceListItemChanged(e);
        }

        private void OnSourceListPropertyDescriptorAdded(ListChangedEventArgs e)
        {
            _pdc = ListUtil.GetItemProperties(SourceList);
            int field = _pdc.IndexOf(e.PropertyDescriptor);
            int columnIndex = field + 1;
            Model.InsertColumns(columnIndex, 1);
            BlinkQueueInsertField(field, null);
            //InsertDirtyCellColumns(columnIndex, 1, null);
            InsertTotalField(field, 1);
            InvalidateVisual(true);
        }

        private void OnSourceListPropertyDescriptorDeleted(ListChangedEventArgs e)
        {
            int field = _pdc.IndexOf(e.PropertyDescriptor);
            _pdc = ListUtil.GetItemProperties(SourceList);
            int columnIndex = field + 1;
            Model.RemoveColumns(columnIndex, 1);
            BlinkQueueDeleteField(field, null);
            //RemoveDirtyCellColumns(columnIndex, 1, null);
            RemoveTotalField(field, 1);
            InvalidateVisual(true);
        }

        private void OnSourceListPropertyDescriptorChanged(ListChangedEventArgs e)
        {
            _pdc = ListUtil.GetItemProperties(SourceList);
            int field = _pdc.IndexOf(e.PropertyDescriptor);
            int columnIndex = field + 1;
            InvalidateCell(GridRangeInfo.Col(columnIndex));
            InvalidateCell(new RowColumnIndex(SourceList.Count + 1, columnIndex));
            if (IsColumnVisible(columnIndex))
                InvalidateVisual(false);
        }

        private void OnSourceListReset(ListChangedEventArgs e)
        {
            // Force getting new PropertDescriptorCollection and refresh everything
            IBindingList list = SourceList;
            this.SourceList = null;
            this.SourceList = list;
            UnloadArrangedCells();
            InvalidateVisual(true);
            this._lastAddNewIndex = -1;
        }

        private void InsertTotalField(int field, int count)
        {
            for (int n = 0; n < count; n++)
                _totals.Insert(field, 0);
        }

        private void RemoveTotalField(int field, int count)
        {
            for (int n = 0; n < count; n++)
                _totals.RemoveAt(field);
        }

        private void PrepareItemAdded(object row)
        {
            OnPrepareItemAdded(row);
        }

        private void OnPrepareItemAdded(object row)
        {
        }
        #region Blinking
        int _blinkTime = 700;

        BlinkTable _blinkTable = new BlinkTable();
        List<BlinkInfo> _blinkQueue = new List<BlinkInfo>();
        int _tickCountAtLastProcessBlinkQueue;
        private void Blink(ListChangedEventArgs e)
        {
            if (BlinkTime <= 0)
                return;
            ChangedFieldInfoCollection changeFields = GetChangedFields();
            bool anyCellInvalidated = false;
            if (changeFields != null)
            {
                foreach (ChangedFieldInfo ci in changeFields)
                {
                    PropertyDescriptor pd = _pdc[ci.Name];
                    object item = SourceList[e.NewIndex];
                    DataRow row = item as DataRow;
                    BlinkState bs = BlinkState.None;
                    if (e.ListChangedType == ListChangedType.ItemAdded)
                        bs = BlinkState.NewRecord;
                    else
                    {
                        object value = ci.NewValue;
                        object oldValue = ci.OldValue;
                        int cmp = CompareColumns.CompareNullableObjects(value, oldValue);
                        if (cmp > 0)
                        {
                            bs = BlinkState.Increased;
                            if (oldValue == null || oldValue is DBNull)
                                bs = BlinkState.NewValue;
                        }
                        else if (cmp < 0)
                        {
                            bs = BlinkState.Reduced;
                            if (value == null || value is DBNull)
                                bs = BlinkState.NullValue;
                        }
                    }
                    if (bs != BlinkState.None)
                    {
                        RowColumnIndex key = BlinkInfo.GetKey(e.NewIndex, ci.FieldIndex);
                        BlinkInfo biOld;
                        if (_blinkTable.TryGetValue(key, out biOld))
                        {
                            int n = _blinkQueue.IndexOf(biOld);
                            if (n != -1)
                                _blinkQueue.RemoveAt(n);
                        }
                        BlinkInfo bi = new BlinkInfo(bs, e.NewIndex, ci.FieldIndex, Environment.TickCount, ci);
                        _blinkQueue.Add(bi);
                        _blinkTable[key] = bi;
                        int record = e.NewIndex;
                        int rowIndex = record + 1;
                        int columnIndex = bi.fieldIndex + 1;
                        RowColumnIndex cellRowColumnIndex = new RowColumnIndex(rowIndex, columnIndex);
                        RenderStyles.Clear(cellRowColumnIndex);
                        if (IsCellVisible(cellRowColumnIndex))
                        {
                            InvalidateCellRenderStyleBackground(cellRowColumnIndex);
                            anyCellInvalidated = true;
                        }
                    }
                }
            }
            if (anyCellInvalidated)
                InvalidateVisual(false);
        }
        void ProcessBlinkQueue()
        {
            int curentTickCount = Environment.TickCount;
            _tickCountAtLastProcessBlinkQueue = curentTickCount;
            bool anyCellInvalidated = false;

            while (_blinkQueue.Count > 0 && curentTickCount - _blinkQueue[0].tickCount > BlinkTime)
            {
                BlinkInfo bi = _blinkQueue[0];
                _blinkQueue.RemoveAt(0);

                if (bi.recordIndex == -1 || bi.fieldIndex == -1) // associated record or column was deleted.
                    continue;

                BlinkInfo bi2 = (BlinkInfo)_blinkTable[bi.GetKey()];
                if (bi2 != null && bi2.tickCount == bi.tickCount)
                    _blinkTable.Clear(bi.GetKey());

                int record = bi.recordIndex;
                int rowIndex = record + 1;
                int columnIndex = bi.fieldIndex + 1;
                RowColumnIndex cellRowColumnIndex = new RowColumnIndex(rowIndex, columnIndex);

                RenderStyles.Clear(cellRowColumnIndex);
                if (IsCellVisible(cellRowColumnIndex))
                {
                    InvalidateCellRenderStyleBackground(cellRowColumnIndex);
                    anyCellInvalidated = true;
                }
            }

            if (anyCellInvalidated)
                InvalidateVisual(false);
        }

        private void BlinkQueueInsertRecord(int index, BlinkTable moveBlinks)
        {
            _blinkTable.InsertRows(index, 1, moveBlinks);
        }
        private void BlinkQueueDeleteRecord(int index, BlinkTable moveBlinks)
        {
            _blinkTable.RemoveRows(index, 1, moveBlinks);
        }
        void BlinkQueueInsertField(int index, BlinkTable moveBlinks)
        {
            _blinkTable.InsertColumns(index, 1, moveBlinks);
        }

        void BlinkQueueDeleteField(int index, BlinkTable moveBlinks)
        {
            _blinkTable.RemoveColumns(index, 1, moveBlinks);
        }
        public BlinkState GetBlinkState(int r, int fd)
        {
            RowColumnIndex key = BlinkInfo.GetKey(r, fd);

            BlinkInfo bi;
            if (_blinkTable.TryGetValue(key, out bi))
                return bi.BlinkState;

            return BlinkState.None;
        }
        #endregion
        #region  Changed Fields
        Hashtable _changedFields = new Hashtable();
        private ChangedFieldInfoCollection _changedFieldsArray = new ChangedFieldInfoCollection();

        public ChangedFieldInfoCollection ChangedFieldsArray
        {
            get { return _changedFieldsArray; }
        }
        public void AddChangedField(ChangedFieldInfo ci)
        {
            if (!_changedFields.Contains(ci.FieldIndex))
            {
                _changedFields[ci.FieldIndex] = ci;
                _changedFieldsArray.Add(ci);
            }
            else
            {
                ChangedFieldInfo ci0 = (ChangedFieldInfo)_changedFields[ci.FieldIndex];
                ci0.NewValue = ci.NewValue;
            }
        }
        public ChangedFieldInfoCollection GetChangedFields()
        {
            return _changedFieldsArray;
        }
        public void ClearChangedFields()
        {
            _changedFields.Clear();
            _changedFieldsArray.Clear();
        }
        #endregion
        private void ComponentDispatcher_ThreadIdle(object? sender, EventArgs e)
        {
            if (IsLoaded && Environment.TickCount - _tickCountAtLastProcessBlinkQueue > 100)
                ProcessBlinkQueue();
        }

        private void UnwireSourceList()
        {
            System.Windows.Interop.ComponentDispatcher.ThreadIdle -= new EventHandler(ComponentDispatcher_ThreadIdle);
            SourceList.ListChanged -= new ListChangedEventHandler(SourceList_ListChanged);
            UnwireDataTable(_dt);
            Model.RowCount = 1;
            Model.ColumnCount = 1;
        }

        private void UnwireDataTable(DataTable dt)
        {
            if (dt == null)
                return;

            dt.ColumnChanging -= new DataColumnChangeEventHandler(Dt_ColumnChanging);
            dt.RowDeleting -= new DataRowChangeEventHandler(Dt_RowDeleting);
        }

        public override void Dispose(bool disposing)
        {
            this.UnwireSourceList();
            this.UnwireModel();
            base.Dispose(disposing);
        }
    }
    internal class BlinkInfo
    {
        public BlinkState BlinkState;
        public int tickCount;
        public int recordIndex;
        public int fieldIndex;
        public ChangedFieldInfo ci;
        public BlinkInfo(BlinkState state, int recordIndex, int fieldIndex, int tickCount, ChangedFieldInfo ci)
        {
            this.BlinkState = state;
            this.recordIndex = recordIndex;
            this.fieldIndex = fieldIndex;
            this.tickCount = tickCount;
            this.ci = ci;
        }
        public RowColumnIndex GetKey()
        {
            return new RowColumnIndex(recordIndex, fieldIndex);
        }
        public static RowColumnIndex GetKey(int record, int fieldIndex)
        {
            return new RowColumnIndex(record, fieldIndex);
        }
        internal BlinkInfo Copy()
        {
            return new BlinkInfo(BlinkState, recordIndex, fieldIndex, tickCount, ci);
        }
    }
    internal class BlinkTable : RowColumnIndexValueDictionary<BlinkInfo>, IRowColumnIndexValueDictionaryCallbacks<BlinkInfo>
    {
        public BlinkTable()
        {
            SetCallback(this);
        }

        public void OnMovedCell(RowColumnIndex cellRowColumnIndex, BlinkInfo value)
        {
            value.recordIndex = cellRowColumnIndex.RowIndex;
            value.fieldIndex = cellRowColumnIndex.ColumnIndex;
        }

        public void OnRemoveCell(RowColumnIndex cellRowColumnIndex, BlinkInfo value)
        {
            value.fieldIndex = value.recordIndex = -1;
        }
    }
}
