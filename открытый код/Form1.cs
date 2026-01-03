using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ZapretApp
{
    public partial class Form1 : Form
    {
        private Process currentProcess = null;
        private int altIndex = 1;

        private readonly string[] altFiles = new string[]
        {
            "general (ALT).bat",
            "general (ALT2).bat",
            "general (ALT3).bat",
            "general (ALT4).bat",
            "general (ALT5).bat",
            "general (ALT6).bat",
            "general.bat",
            "general (ALT7).bat",
            "general (ALT8).bat",
            "general (ALT9).bat"
        };

        public Form1()
        {
            InitializeComponent();
            UpdateStatus("Готов", Color.Gray);
        }

        private string GetMainFolderPath() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "main");

        private void UpdateStatus(string text, Color? color = null)
        {
            lblStatus.Text = $"⚡ {text}";
            lblStatus.ForeColor = color ?? Color.FromArgb(52, 58, 64);
        }

        private void RunBat(string filename, bool visibleConsole = false)
        {
            StopBat();

            try
            {
                string basePath = GetMainFolderPath();
                string filePath = Path.Combine(basePath, filename);

                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Файл \"{filename}\" не найден в папке main.\nПуть: {filePath}",
                        "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("Файл не найден", Color.Red);
                    return;
                }

                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = basePath,
                    Arguments = $"/c \"{filePath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = !visibleConsole,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = visibleConsole ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
                };

                currentProcess = new Process();
                currentProcess.StartInfo = psi;
                currentProcess.EnableRaisingEvents = true;

                if (!visibleConsole)
                {
                    currentProcess.OutputDataReceived += (s, e) => { };
                    currentProcess.ErrorDataReceived += (s, e) => { };
                }

                currentProcess.Start();

                if (!visibleConsole)
                {
                    currentProcess.BeginOutputReadLine();
                    currentProcess.BeginErrorReadLine();
                }

                // Проверяем, запустился ли процесс успешно
                if (!currentProcess.HasExited)
                {
                    if (filename == "general (ALT10).bat")
                        UpdateStatus("Подключено ✓", Color.Green);
                    else
                        UpdateStatus($"Запущен: {filename}", Color.Blue);
                }
                else
                {
                    UpdateStatus("Ошибка запуска ✗", Color.Red);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось запустить \"{filename}\".\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Ошибка запуска ✗", Color.Red);
            }
        }

        private void StopBat()
        {
            try
            {
                if (currentProcess != null && !currentProcess.HasExited)
                {
                    try { currentProcess.Kill(); } catch { }
                }
            }
            finally
            {
                if (currentProcess != null)
                {
                    currentProcess.Dispose();
                    currentProcess = null;
                }
                UpdateStatus("Остановлен ⚠", Color.Orange);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            RunBat("general (ALT10).bat");
            btnConnect.Text = "ПОДКЛЮЧЕНО!";
            btnConnect.BackColor = Color.FromArgb(40, 167, 69);
            btnDisconnect.Visible = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            StopBat();
            btnConnect.Text = "ПОДКЛЮЧИТЬСЯ";
            btnConnect.BackColor = Color.FromArgb(0, 122, 204);
            btnDisconnect.Visible = false;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            RunBat("service.bat", visibleConsole: true);
        }

        private void btnNotWorking_Click(object sender, EventArgs e)
        {
            StopBat();
            string nextBat = altFiles[altIndex - 1];
            RunBat(nextBat);

            altIndex++;
            if (altIndex > altFiles.Length) altIndex = 1;
        }

        private void btnTelegram_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://t.me/d511developers",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://t.me/d511developers",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}