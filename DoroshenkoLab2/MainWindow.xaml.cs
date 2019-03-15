using System.Windows;
using FontAwesome.WPF;

namespace DoroshenkoLab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {
        private ImageAwesome _loader;
        private MainInputView _mainInputView;
        private ResultView _resultView;

        public MainWindow()
        {
            InitializeComponent();
            ShowInputView();
            DataContext = new WindowModel(ShowResultView, ShowInputView, ShowLoader);
        }

        private void ShowResultView()
        {
            ShowtView(ref _resultView);
        }

        private void ShowInputView()
        {
            ShowtView(ref _mainInputView);
        }

        private void ShowLoader(bool isShow)
        {
            LoaderHelper.OnRequestLoader(MainGrid, ref _loader, isShow);
        }

        private void ShowtView<TObject>(ref TObject view) where TObject : UIElement, new()
        {
            MainGrid.Children.Clear();
            if (view == null){
                view = new TObject();
            }
            MainGrid.Children.Add(view);
        }


    }
}
