using System;
using System.IO;
using System.Threading.Tasks;
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

		public async void OnPlainTextOpenButton(Window window)
		{
			var path = await GetFilePathOpen(window);

			if (path != null) Console.WriteLine("PlainText: " + path);
		}
		
		public async void OnKeyOpenButton(Window window)
		{
			var path = await GetFilePathOpen(window);

			if (path != null) Console.WriteLine("Key: " + path);
		}
		
		public async void OnCipherOpenButton(Window window)
		{
			var path = await GetFilePathOpen(window);

			if (path != null) Console.WriteLine("Cipher: " + path);
		}
		
		public async void OnPlainTextSaveButton(Window window)
		{
			var path = await GetFilePathSave(window);

			if (path != null) Console.WriteLine("PlainText: " + path);
		}
		
		public async void OnKeySaveButton(Window window)
		{
			var path = await GetFilePathSave(window);

			if (path != null) Console.WriteLine("Key: " + path);
		}
		
		public async void OnCipherSaveButton(Window window)
		{
			var path = await GetFilePathSave(window);

			if (path != null) Console.WriteLine("Cipher: " + path);
		}

		private async Task<string?> GetFilePathOpen(Window window)
		{
			var dialog = new OpenFileDialog()
			{
				Title = "Otwórz plik",
				AllowMultiple = false
			};

			var result = await dialog.ShowAsync(window);
			return result?[0];
		}
		
		private async Task<string?> GetFilePathSave(Window window)
		{
			var dialog = new SaveFileDialog()
			{
				Title = "Zapisz plik",
			};

			var result = await dialog.ShowAsync(window);
			return result;
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
		
	}
}