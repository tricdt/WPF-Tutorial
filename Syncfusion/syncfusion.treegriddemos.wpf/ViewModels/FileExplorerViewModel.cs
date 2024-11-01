
using System.IO;

namespace syncfusion.treegriddemos.wpf
{
    public class FileExplorerViewModel
    {
        public FileExplorerViewModel()
        {
            this.DriveDetails = this.GetRootDrives();
        }


        #region Properties
        private List<FileInfoModel> _driveDetails;
        /// <summary>
        /// Get or set the DriveDetails
        /// </summary>
        public List<FileInfoModel> DriveDetails
        {
            get { return _driveDetails; }
            set { _driveDetails = value; }
        }
        #endregion
        #region Methods
        private List<FileInfoModel> GetRootDrives()
        {
            List<FileInfoModel> drives = new List<FileInfoModel>();
            foreach (string s in Directory.GetLogicalDrives())
            {
                try
                {
                    drives.Add(Infomodel(s));
                }
                catch (Exception) { }
            }
            return drives;
        }
        public FileInfoModel Infomodel(string path)
        {
            FileInfoModel model = new FileInfoModel();
            FileInfo fi = new FileInfo(path);
            model.FullName = Path.GetFullPath(path);
            model.ShortName = Path.GetFileNameWithoutExtension(path);
            if (model.ShortName == "")
            {
                model.ShortName = path;
                model.FileType = "DriveNode";
                System.IO.DriveInfo di = new System.IO.DriveInfo(path);
                model.TotalSize = (di.TotalSize / 1073741824).ToString();
                var freeSpace = (double.Parse(di.TotalFreeSpace.ToString()) / 1073741824);
                model.TotalFreeSpace = (Math.Round(freeSpace, 1)).ToString();
                model.PercentofFreeSpace = 100 - ((double.Parse(model.TotalFreeSpace) / double.Parse(model.TotalSize)) * 100);
            }
            else
            {
                if ((fi.Attributes & FileAttributes.Directory) != 0)
                {
                    model.FileType = "Directory";
                }
                else
                {
                    model.Size = fi.Length.ToString() + "Kb";
                    model.FileType = Path.GetExtension(path);
                }
            }
            model.DateModified = fi.LastWriteTime;
            model.DateAccessed = fi.LastAccessTime;
            model.DateCreated = fi.CreationTime;
            model.Attributes = fi.Attributes;
            return model;
        }

        public List<FileInfoModel> GetChildFolderContent(FileInfoModel fileNodeItem)
        {
            List<FileInfoModel> children = new List<FileInfoModel>();

            string folder = fileNodeItem.FullName;

            try
            {
                FileInfo fi = new FileInfo(folder);
                if ((fi.Attributes & FileAttributes.Directory) != (FileAttributes)0)
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    // Skip Recycle Bin, System Volume Information etc.
                    if (di.Parent != null && (di.Attributes & FileAttributes.Hidden) != (FileAttributes)0
                        || (int)di.Attributes == -1)
                    {
                        //skip...
                    }
                    else
                    {
                        foreach (string s2 in Directory.GetDirectories(folder))
                        {
                            FileInfo fi2 = new FileInfo(s2);
                            if ((fi2.Attributes & FileAttributes.Hidden) != (FileAttributes)0)
                                continue;
                            children.Add(Infomodel(s2));
                        }
                        foreach (string s2 in Directory.GetFiles(folder))
                        {
                            FileInfo fi2 = new FileInfo(s2);
                            if ((fi2.Attributes & FileAttributes.Hidden) != (FileAttributes)0)
                                continue;
                            children.Add(Infomodel(s2));
                        }
                    }
                }
            }
            catch { }
            return children;
        }
        #endregion
    }
}
