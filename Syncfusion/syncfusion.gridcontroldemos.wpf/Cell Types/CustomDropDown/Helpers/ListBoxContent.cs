﻿using System.Windows.Media.Imaging;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ListBoxContent
    {
        private List<ImageTextListBoxListBoxItem> allDatas = new List<ImageTextListBoxListBoxItem>();

        public List<ImageTextListBoxListBoxItem> GenerateListBoxContent()
        {
            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/Sunset.jpg", UriKind.Relative)),
                Text = "Sunset"
            });

            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/Water lilies.jpg", UriKind.Relative)),

                Text = "Water lilies"
            });

            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/Winter.jpg", UriKind.Relative)),

                Text = "Winter"
            });

            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/Mixed Flowers and a Bear.jpg", UriKind.Relative)),

                Text = "Mixed Flowers"
            });

            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/Bidens_discoidea_flowers3.jpg", UriKind.Relative)),

                Text = "Bidens_discoidea_flowers"
            });

            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/Dichelostemma_capitatum.jpg", UriKind.Relative)),

                Text = "Dichelostemma_capitatum,_flowers"
            });

            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/Dahlia-flowers.jpg", UriKind.Relative)),

                Text = "Dahlia"
            });

            this.allDatas.Add(new ImageTextListBoxListBoxItem()
            {
                Image = new BitmapImage(
                    new Uri(@"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flowers/flowers4.jpg", UriKind.Relative)),
                Text = "Violet"
            });

            return this.allDatas;
        }
    }
}
