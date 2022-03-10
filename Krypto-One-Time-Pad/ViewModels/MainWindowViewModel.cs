using System;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Krypto_One_Time_Pad.Models.Daos;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace Krypto_One_Time_Pad.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
	private string cipherText = "Tutaj możesz wpisać szyfrogram w formie UTF-8";
	private int cipherTextLenght;
	private string key = "Tutaj możesz wpisać klucz w formie UTF-8";
	private int keyLenght;

	private string plainText = "Tutaj możesz wpisać tekst jawny w formie UTF-8";
	private int plainTextLenght;

	public MainWindowViewModel()
	{
		PlainTextLenght = plainText.Length;
		CipherTextLenght = plainText.Length;
		KeyLenght = key.Length;
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

	private async void OnAuthorsClick()
	{
		var msBoxStandardWindow = MessageBoxManager
			.GetMessageBoxStandardWindow(new MessageBoxStandardParams
			{
				ButtonDefinitions = ButtonEnum.Ok,
				ContentTitle = "Autorzy",
				ContentMessage = "Szymon Świędrych \nMichalina Śledzikowska",
				Style = Style.DarkMode
			});
		await msBoxStandardWindow.Show();
	}

	public async void OnPlainTextOpenButton(Window window)
	{
		var path = await GetFilePathOpen(window);
		if (path == null) return;
		PlainText = ReadFile(path);
	}

	

	public async void OnKeyOpenButton(Window window)
	{
		var path = await GetFilePathOpen(window);
		if (path == null) return;
		Key = ReadFile(path);
	}

	public async void OnCipherOpenButton(Window window)
	{
		var path = await GetFilePathOpen(window);
		if (path == null) return;
		Key = ReadFile(path);
	}
	
	public async void OnPlainTextSaveButton(Window window)
	{
		var path = await GetFilePathSave(window);
		if (path == null) return;
		WriteFile(path, PlainText);
	}

	public async void OnKeySaveButton(Window window)
	{
		var path = await GetFilePathSave(window);
		if (path == null) return;
		WriteFile(path, Key);
	}

	public async void OnCipherSaveButton(Window window)
	{
		var path = await GetFilePathSave(window);
		if (path == null) return;
		WriteFile(path, CipherText);
	}

	private string ReadFile(string path)
	{
		IDao dao = new FileDao(path);
		var bytes = dao.Read();
		return Convert.ToBase64String(bytes);
	}
	
	private void WriteFile(string path, string content)
	{
		IDao dao = new FileDao(path);
		//var bytes = Encoding.UTF8.GetBytes(content);
		var bytes = Convert.FromBase64String(content);
		dao.Write(bytes);
	}
	
	private async Task<string?> GetFilePathOpen(Window window)
	{
		var dialog = new OpenFileDialog
		{
			Title = "Otwórz plik",
			AllowMultiple = false
		};

		var result = await dialog.ShowAsync(window);
		return result?[0];
	}

	private async Task<string?> GetFilePathSave(Window window)
	{
		var dialog = new SaveFileDialog
		{
			Title = "Zapisz plik"
		};

		var result = await dialog.ShowAsync(window);
		return result;
	}
}