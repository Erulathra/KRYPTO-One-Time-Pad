using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Krypto_One_Time_Pad.ViewModels;
using ReactiveUI;

namespace Krypto_One_Time_Pad.Views
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = new MainWindowViewModel();
			var context = this.DataContext as MainWindowViewModel;
			context.PlainText = "Ziemniak";
		}

		
	}
}