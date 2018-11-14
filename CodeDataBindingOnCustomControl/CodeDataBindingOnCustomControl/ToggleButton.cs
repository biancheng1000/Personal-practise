using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeDataBindingOnCustomControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CodeDataBindingOnCustomControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CodeDataBindingOnCustomControl;assembly=CodeDataBindingOnCustomControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ToggleButton/>
    ///
    /// </summary>
    public class ToggleButton : RadioButton
    {
        static ToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButton), new FrameworkPropertyMetadata(typeof(ToggleButton)));
        }

        public ToggleButton()
        {
          
        }

        #region MyProperty 注释

        public static readonly DependencyProperty ConvertParamProperty = DependencyProperty.Register("ConvertParam", typeof(string), typeof(ToggleButton),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,CallBack));

        private static void CallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleButton control = d as ToggleButton;
            Binding ischeckBinding = new Binding("DataContext.SelectMenu");
            ischeckBinding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1);
            ischeckBinding.NotifyOnTargetUpdated = true;
            ischeckBinding.NotifyOnSourceUpdated = true;
            ischeckBinding.Mode = BindingMode.TwoWay;
            ischeckBinding.Converter = new ConverterEnumToBool();
            ischeckBinding.ConverterParameter = e.NewValue;
            control.SetBinding(RadioButton.IsCheckedProperty, ischeckBinding);
        }

        /// <summary>
        /// 注释
        /// </summary>
        public string ConvertParam
        {
            get
            {
                return (string)GetValue(ConvertParamProperty);
            }
            set
            {
                SetValue(ConvertParamProperty, value);
            }
        }
        #endregion
    }


    public class ConverterEnumToBool : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string ParameterString = parameter as string;
            if (ParameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object paramvalue = Enum.Parse(value.GetType(), ParameterString) ?? new object();

            return paramvalue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (!isChecked)
            {
                //这里需要注意ConvertBack方法中判断value的值为false的时候，会直接返回null
                //这样写是为了RadioButton的状态变为未选中的时候，阻止数据传回Employee的实例。
                //如果不这样做，值更新会在两个RadioButton之间形成一个环路，导致RadioButton不能正常工作。
                return null;
            }

            string ParameterString = parameter as string;
            if (ParameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, ParameterString);
        }

        #endregion
    }
}
