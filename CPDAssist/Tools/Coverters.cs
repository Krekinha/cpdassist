using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using CPDAssist.MDFe;

namespace CPDAssit.Tools
{
    public class DangerConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parametro, CultureInfo culture)
        {
            /*if (parametro.ToString() == "returnImage")
            {
                return retornaImagem((string)value);
            }
            else
            {*/
            if (value.ToString() == "")
                return new SolidColorBrush(Color.FromRgb(249, 73, 73));
            else
            {

                if ((DateTime.Parse(value.ToString()) < (DateTime.Now.Subtract(TimeSpan.FromMinutes(10)))))
                {
                    return new SolidColorBrush(Color.FromRgb(249, 73, 73));
                }
                else
                {
                    return new SolidColorBrush(Colors.SpringGreen);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /*private DrawingImage retornaImagem(string situacao)
        {
            ResourceDictionary dic = new ResourceDictionary();
            dic = (ResourceDictionary)Application.LoadComponent(new Uri("/SRO;component/Resources/Dictionaries/dicVectors.xaml", UriKind.RelativeOrAbsolute));

            if (situacao == "Entrega Efetuada")
            {
                DrawingImage img = (DrawingImage)dic["imgEnviado"];
                return img;
            }
            else if (situacao == "Encaminhado")
            {
                DrawingImage img = (DrawingImage)dic["imgEncaminhado"];
                return img;
            }
            else
            {
                return null;
            }
        }*/
    }
    public class ContentToPathConverter : IValueConverter

    {
        readonly static ContentToPathConverter value = new ContentToPathConverter();
        public static ContentToPathConverter Value
        {
            get { return value; }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ContentPresenter cp = (ContentPresenter)value;
            double h = cp.ActualHeight > 10 ? 1.4 * cp.ActualHeight : 10;
            double w = cp.ActualWidth > 10 ? 1.25 * cp.ActualWidth : 10;
            PathSegmentCollection ps = new PathSegmentCollection(4)
        {
            new LineSegment(new Point(1, 0.7 * h), true),
            new BezierSegment(new Point(1, 0.9 * h), new Point(0.1 * h, h), new Point(0.3 * h, h), true),
            new LineSegment(new Point(w, h), true),
            new BezierSegment(new Point(w + 0.6 * h, h), new Point(w + h, 0), new Point(w + h * 1.3, 0), true)
        };
            PathFigure figure = new PathFigure(new Point(1, 0), ps, false);
            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            return geometry;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class ContentToMarginConverter : IValueConverter
    {
        readonly static ContentToMarginConverter value = new ContentToMarginConverter();
        public static ContentToMarginConverter Value
        {
            get { return value; }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness(0, 0, -((ContentPresenter)value).ActualHeight, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class SituacaoCNPJConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            {
                try
                {
                    if (values[0].ToString() == "HABILITADO")
                    {
                        return new SolidColorBrush(Color.FromArgb(255, 8, 241, 40));
                    }else if ((values[0].ToString() == "REJEIÇÃO"))
                    {
                        return new SolidColorBrush(Color.FromArgb(255, 236, 53, 53));
                    }
                    else if ((values[0].ToString() == "ERRO"))
                    {
                        return new SolidColorBrush(Color.FromArgb(255, 177, 97, 243));
                    }
                    else
                    {
                        return new SolidColorBrush(Color.FromArgb(100, 112, 112, 112));
                    }

                }
                catch
                {
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }


            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TotalNFValorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = (ReadOnlyObservableCollection<object>)value;
            if (items == null) return "";

            var total = items.Cast<NFMDFe>().Sum(nf => nf.ValorNF);
            return total.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class TotalNFPesoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = value as ReadOnlyObservableCollection<object>;
            if (items == null) return "";

            var total = items.Cast<NFMDFe>().Sum(nf => nf.Peso);
            return total.ToString("N4", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}

