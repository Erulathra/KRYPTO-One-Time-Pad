using System;
using Avalonia.Controls;
using JetBrains.Annotations;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace Krypto_One_Time_Pad.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		
		private string plainText = "Tutaj możesz wpisać tekst jawny w formie UTF-8";
		private int plainTextLenght = 0;
		private string cipherText = "Tutaj możesz wpisać szyfrogram w formie UTF-8";
		private int cipherTextLenght = 0;
		private string key  = "Tutaj możesz wpisać klucz w formie UTF-8";
		private int keyLenght = 0;

		private string plainTextPath = "ścieżka";
		public string CipherPath { get; set; } = "ścieżka";
		public string KeyPath { get; set; } = "ścieżka";


		private async void OnAuthorsClick()
		{
			var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
				.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
					ButtonDefinitions = ButtonEnum.Ok,
					ContentTitle = "Autorzy",
					ContentMessage = "Szymon Świędrych \nMichalina Śledzikowska",
					Style=Style.DarkMode
				});
			await msBoxStandardWindow.Show();
		}

		public MainWindowViewModel()
		{
			PlainTextLenght = plainText.Length;
			CipherTextLenght = plainText.Length;
			KeyLenght = key.Length;
		}

		public async void OnTest(Window window)
		{
			var dialog = new OpenFileDialog()
			{
				Title = "Wybierz plik",
				AllowMultiple = false
			};

			var result = await dialog.ShowAsync(window);
			PlainTextPath = result[0];
		}

		public string PlainText
		{
			get => plainText;
			set
			{
				this.RaiseAndSetIfChanged(ref plainText, value);
				PlainTextLenght = plainText.Length;
			}
		}

		public int PlainTextLenght
		{
			get => plainTextLenght;
			set => this.RaiseAndSetIfChanged(ref plainTextLenght, value);
		}
		public string CipherText
		{
			get => cipherText;
			set
			{
				this.RaiseAndSetIfChanged(ref cipherText, value);
				CipherTextLenght = CipherText.Length;
			}
		}

		public int CipherTextLenght
		{
			get => cipherTextLenght;
			set => this.RaiseAndSetIfChanged(ref cipherTextLenght, value);
		}
		
		public string Key
		{
			get => key;
			set
			{
				this.RaiseAndSetIfChanged(ref key, value);
				KeyLenght = key.Length;
			}
		}

		public int KeyLenght
		{
			get => keyLenght;
			set => this.RaiseAndSetIfChanged(ref keyLenght, value);
		}

		public string PlainTextPath
		{
			get => plainTextPath;
			set => this.RaiseAndSetIfChanged(ref plainTextPath, value);
		}
	}
}