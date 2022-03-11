using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Krypto_One_Time_Pad.Models.Daos;
using Krypto_One_Time_Pad.Models.OneTimePad;
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

	private IOneTimePad oneTimePad = new OneTimePad();

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

	private async void OnAuthorsButton()
	{
		var msBoxStandardWindow = MessageBoxManager
			.GetMessageBoxStandardWindow(new MessageBoxStandardParams
			{
				ButtonDefinitions = ButtonEnum.Ok,
				ContentTitle = "Autorzy",
				ContentMessage = "Szymon Świędrych \nMichalina Śledzikowska",
				Style = Style.DarkMode,
				Icon = Icon.Info
			});
		await msBoxStandardWindow.Show();
	}

	private async void OnGenerateKeyButton()
	{
		var buttonResult = await ShowGenerateKeyMessageBox();
		if (buttonResult != ButtonResult.Ok) 
			return;

		var tempPlainText = Convert.FromBase64String(PlainText);
		
		var byteKey = oneTimePad.GenerateKey(tempPlainText.Length);
		Key = Convert.ToBase64String(byteKey);
	}

	private static async Task<ButtonResult> ShowGenerateKeyMessageBox()
	{
		var msBoxStandardWindow = MessageBoxManager
			.GetMessageBoxStandardWindow(new MessageBoxStandardParams
			{
				ButtonDefinitions = ButtonEnum.OkCancel,
				ContentTitle = "Generowanie Klucza",
				ContentMessage =
					"Uwaga!, metoda generowania klucza jest psełdolosowa i nie powinno się" +
					"\nz niej korzystać do generowania klucza. Czy mimo to chcesz kontynuować?",
				Icon = Icon.Warning,
				Style = Style.DarkMode
			});
		var result = await msBoxStandardWindow.Show();
		return result;
	}
#pragma warning disable CS4014

	public void OnEncryptButton()
	{
		var plainTextBytes = Convert.FromBase64String(PlainText);
		var keyBytes = Convert.FromBase64String(Key);
		try
		{
			var tempCipher = oneTimePad.Encrypt(plainTextBytes, keyBytes);
			CipherText = Convert.ToBase64String(tempCipher);

		}
		catch (OneTimePadException e)
		{
			ShowExceptionMessageBox(e);
		}
		
	}
	
	public void OnDecryptButton()
	{
		var cipherBytes = Convert.FromBase64String(CipherText);
		var keyBytes = Convert.FromBase64String(Key);
		try
		{
			var tempPlainText = oneTimePad.Decrypt(cipherBytes, keyBytes);
			plainText = Convert.ToBase64String(tempPlainText);
		}
		catch (OneTimePadException e)
		{
			ShowExceptionMessageBox(e);
		}
	}
#pragma warning restore CS4014

	private static async Task<ButtonResult> ShowExceptionMessageBox(OneTimePadException e)
	{
		var msBoxStandardWindow = MessageBoxManager
			.GetMessageBoxStandardWindow(new MessageBoxStandardParams
			{
				ButtonDefinitions = ButtonEnum.Ok,
				ContentTitle = "Błąd",
				ContentMessage = e.Message,
				Icon = Icon.Error,
				Style = Style.DarkMode
			});
		var result = await msBoxStandardWindow.Show();
		return result;
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