﻿using System.Windows;

namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// This class converts a collection size to visibility.
    /// </summary>
    public class CollectionVisibilityConverter : EmptyCollectionToObjectConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionVisibilityConverter"/> class.
        /// </summary>
        public CollectionVisibilityConverter()
        {
            NotEmptyValue = Visibility.Visible;
            EmptyValue = Visibility.Collapsed;
        }
    }
}
