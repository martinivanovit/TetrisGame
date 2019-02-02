using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace TetrisApp
{
    public partial class GlyphButton : UserControl
    {
        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }
        
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register("Glyph", typeof(string), typeof(GlyphButton), new PropertyMetadata(string.Empty));
        
        public Brush GlyphForeground
        {
            get { return (Brush)GetValue(GlyphForegroundProperty); }
            set { SetValue(GlyphForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GlyphForegroundProperty =
            DependencyProperty.Register("GlyphForeground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(null, OnGlyphForegroundChanged));

        private static void OnGlyphForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (GlyphButton)d;
            button.textBlock.Foreground = (Brush)e.NewValue;
        }

        public Brush GlyphMouseOverForeground
        {
            get { return (Brush)GetValue(GlyphMouseOverForegroundProperty); }
            set { SetValue(GlyphMouseOverForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GlyphMouseOverForegroundProperty =
            DependencyProperty.Register("GlyphMouseOverForeground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(null));
        
        public GlyphButton()
        {
            InitializeComponent();
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.textBlock.Foreground = this.GlyphMouseOverForeground;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.textBlock.Foreground = this.GlyphForeground;
        }
    }
}
