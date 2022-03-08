using Avalonia.Controls;
using Krypto_One_Time_Pad.ViewModels;

namespace Krypto_One_Time_Pad.Views
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = new MainWindowViewModel();
		}
		
	}
}