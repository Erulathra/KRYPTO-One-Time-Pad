using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using JetBrains.Annotations;
using Krypto_One_Time_Pad.Models.Daos;
using Krypto_One_Time_Pad.Models.OneTimePad;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace Krypto_One_Time_Pad.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
	private byte[] cipherBytes;
	private string cipherText = "Tutaj możesz wpisać szyfrogram w formie UTF-8";
	private string cipherFilePath = "";
	private int cipherTextLenght;

	private string key = "Tutaj możesz wpisać klucz w formie UTF-8";
	private string keyFilePath = "";
	private byte[] keyBytes;
	private int keyLenght;

	private readonly IOneTimePad oneTimePad = new OneTimePad();

	private string plainText = "Tutaj możesz wpisać tekst jawny w formie UTF-8";
	private byte[] plainTextBytes;
	private string plainFilePath = "";
	private int plainTextLenght;

	private bool isEncryptText = false;

	public MainWindowViewModel()
	{
		CipherText = "Tutaj możesz wpisać szyfrogram w formie UTF-8";
		Key = "Tutaj możesz wpisać klucz w formie UTF-8";
		PlainText = "Tutaj możesz wpisać tekst jawny w formie UTF-8";
	}

	public string PlainText
	{
		get => plainText;
		set
		{
			this.RaiseAndSetIfChanged(ref plainText, value);
			var tempBytes = Encoding.UTF8.GetBytes(plainText);
			PlainTextLenght = tempBytes.Length;
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
			var tempBytes = ConvertStringToByte(cipherText);
			CipherTextLenght = tempBytes.Length;
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
			var tempBytes = Encoding.UTF8.GetBytes(key);
			KeyLenght = tempBytes.Length;
		}
	}

	public int KeyLenght
	{
		get => keyLenght;
		set => this.RaiseAndSetIfChanged(ref keyLenght, value);
	}

	public bool IsEncryptText
	{
		get => isEncryptText;
		set => this.RaiseAndSetIfChanged(ref isEncryptText, value);
	}

	public string CipherFilePath
	{
		get => cipherFilePath;
		set => this.RaiseAndSetIfChanged(ref cipherFilePath, value);
	}

	public string KeyFilePath
	{
		get => keyFilePath;
		set => this.RaiseAndSetIfChanged(ref keyFilePath, value);
	}

	public string PlainFilePath
	{
		get => plainFilePath;
		set => this.RaiseAndSetIfChanged(ref plainFilePath, value);
	}

	public void OnEncryptTextChange(CheckBox checkBox)
	{
		bool? isChecked = checkBox.IsChecked;
		if(isChecked != null)
			IsEncryptText = (bool)isChecked;
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

		var plainTextLocalBytes = plainTextBytes;
		
		if (isEncryptText)
		{
			plainTextLocalBytes = Encoding.UTF8.GetBytes(key);
		}

		keyBytes = oneTimePad.GenerateKey(plainTextLocalBytes.Length);
		KeyLenght = keyBytes.Length;
		KeyFilePath = "Wygenerowany Klucz";
		
		if (isEncryptText)
		{
			Key = Encoding.UTF8.GetString(keyBytes);
		}
	}

	private static async Task<ButtonResult> ShowGenerateKeyMessageBox()
	{
		var msBoxStandardWindow = MessageBoxManager
			.GetMessageBoxStandardWindow(new MessageBoxStandardParams
			{
				ButtonDefinitions = ButtonEnum.OkCancel,
				ContentTitle = "Generowanie Klucza",
				ContentMessage =
					"Uwaga!, metoda generowania klucza jest pseudolosowa i nie powinno się" +
					"\nz niej korzystać do generowania klucza. Czy mimo to chcesz kontynuować?",
				Icon = Icon.Warning,
				Style = Style.DarkMode
			});
		var result = await msBoxStandardWindow.Show();
		return result;
	}

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
		var bytes= ReadFile(path);
		PlainFilePath = Path.GetFileName(path);
		plainTextBytes = bytes;
		PlainTextLenght = plainTextBytes.Length;
	}

	public async void OnKeyOpenButton(Window window)
	{
		var path = await GetFilePathOpen(window);
		if (path == null) return;
		var bytes= ReadFile(path);
		KeyFilePath = Path.GetFileName(path);
		keyBytes = bytes;
		KeyLenght = keyBytes.Length;
	}

	public async void OnCipherOpenButton(Window window)
	{
		var path = await GetFilePathOpen(window);
		if (path == null) return;
		var bytes= ReadFile(path);
		CipherFilePath = Path.GetFileName(path);
		cipherBytes = bytes;
		CipherTextLenght = cipherBytes.Length;
	}

	public async void OnPlainTextSaveButton(Window window)
	{
		var path = await GetFilePathSave(window);
		if (path == null) return;
		WriteFile(path, plainTextBytes);
	}

	public async void OnKeySaveButton(Window window)
	{
		var path = await GetFilePathSave(window);
		if (path == null) return;
		WriteFile(path, keyBytes);
	}

	public async void OnCipherSaveButton(Window window)
	{
		var path = await GetFilePathSave(window);
		if (path == null) return;
		WriteFile(path, cipherBytes);
	}

	private Byte[] ReadFile(string path)
	{
		IDao dao = new FileDao(path);
		return dao.Read();
	}

	private void WriteFile(string path, Byte[] content)
	{
		IDao dao = new FileDao(path);
		dao.Write(content);
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
#pragma warning disable CS4014

	public void OnEncryptButton()
	{
		var plainTextLocalBytes = plainTextBytes;
		var keyLocalBytes = keyBytes;
		
		if (isEncryptText)
		{
			plainTextLocalBytes = Encoding.UTF8.GetBytes(plainText);
			keyLocalBytes = Encoding.UTF8.GetBytes(key);
		}
		
		try
		{
			var tempCipher = oneTimePad.Encrypt(plainTextLocalBytes, keyLocalBytes);
			if (isEncryptText)
			{
				CipherText = ConvertByteToString(tempCipher);
			}
			else
			{
				CipherFilePath = "Wygenerowany Szyfrogram";
			}
			
			cipherBytes = tempCipher;
			CipherTextLenght = tempCipher.Length;

		}
		catch (OneTimePadException e)
		{
			ShowExceptionMessageBox(e);
		}
	}

	public void OnDecryptButton()
	{
		var cipherLocalBytes = cipherBytes;
		var keyLocalBytes = keyBytes;
		
		if (isEncryptText)
		{
			cipherLocalBytes = ConvertStringToByte(cipherText);
			keyLocalBytes = Encoding.UTF8.GetBytes(key);
		}
		
		try
		{
			var tempPlainText = oneTimePad.Decrypt(cipherLocalBytes, keyLocalBytes);

			if (isEncryptText)
			{
				PlainText = Encoding.UTF8.GetString(tempPlainText);
			}
			else
			{
				PlainFilePath = "Wygenerowany Tekst Jawny";
			}

			plainTextBytes = tempPlainText;
		}
		catch (OneTimePadException e)
		{
			ShowExceptionMessageBox(e);
		}
	}

	private string ConvertByteToString(Byte[] bytes)
	{
		var characters = bytes.Select(b => (char)b).ToArray();
		return new string(characters);
	}
	
	private Byte[] ConvertStringToByte(string str)
	{
		var characters = str.ToCharArray();
		return characters.Select(c => (byte)c).ToArray();
	}
#pragma warning restore CS4014
}