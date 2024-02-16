﻿using System;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Globalization;
using System.Drawing.Text;
using System.ComponentModel.Design;

namespace Car_Editor
{
    public partial class MainForm : Form
    {
        private Button openFileDialogButton; // Deklaracja przycisku OpenFileDialog
        private Label selectedFileLabel; // Deklaracja etykiety na nazwę wybranego pliku
        private ComboBox options; // Wybór opcji 
        private int selectedComboBox;
        private Label LabelOptions; // opis do wyboru opcji
        private CheckBox checkbox;
        private CheckBox checkbox2;
        private Button Submit;
        private string selectedFilePath = "";
        private bool selectedcheckbox1;
        private bool selectedcheckbox2;
        private const string ConfigFilePath = "appsettings.json";
        private ConfigModel config; // Config file

        public MainForm()
        {
            InitializeComponent();
            InitializeOpenFileDialogButton(); // Inicjalizacja przycisku OpenFileDialog
            InitializeSelectedFileLabel(); // Inicjalizacja etykiety na nazwę wybranego pliku
            InitializeOptions(); // Inicjalizacja wyboru opcji
            InitializeOptionsLabel(); // Inicjalizacja label do wyboru opcji
            InitializeCheckbox();
            InitializeSubmit();
            InitializeEditConfigButton();
            config = LoadConfig();

            // Dodanie obsługi zdarzeń przeciągania i upuszczania plików na przycisk
            openFileDialogButton.AllowDrop = true;
            openFileDialogButton.DragEnter += openFileDialogButton_DragEnter;
            openFileDialogButton.DragDrop += openFileDialogButton_DragDrop;

            this.MinimumSize = new System.Drawing.Size(400, 300);
        }

        private ConfigModel LoadConfig()
        {
            try
            {
                // Odczytaj zawartość pliku JSON
                string jsonString = File.ReadAllText(ConfigFilePath);

                // Deserializacja zawartości JSON do obiektu
                return JsonSerializer.Deserialize<ConfigModel>(jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas odczytywania pliku konfiguracyjnego: {ex.Message}");
                return null;
            }
        }

        private void InitializeEditConfigButton()
        {
            // Tworzenie przycisku
            Button editConfigButton = new Button();
            editConfigButton.Text = "Edytuj Config";
            editConfigButton.Dock = DockStyle.Bottom; // Przycisk będzie umieszczony na dole formularza
            editConfigButton.Click += editConfigButton_Click; // Przypisanie zdarzenia kliknięcia

            // Dodanie przycisku do formularza
            this.Controls.Add(editConfigButton);
        }

        private void editConfigButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Otwórz plik appsettings.json w domyślnym edytorze tekstu systemu operacyjnego
                Process.Start(new ProcessStartInfo("notepad.exe", "appsettings.json") { Verb = "runas"});
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas otwierania pliku konfiguracyjnego: {ex.Message}");
            }
    }

        private void InitializeOpenFileDialogButton() // Funkcja odpowiadająca za uruchomienie pojawienia się przycisku
        {
            // Inicjalizacja przycisku
            openFileDialogButton = new Button();
            openFileDialogButton.Text = "Wybierz plik";
            openFileDialogButton.Location = new System.Drawing.Point(10, 10);
            openFileDialogButton.Size = new System.Drawing.Size(100, 30);
            openFileDialogButton.Click += openFileDialogButton_Click; // Przypisanie zdarzenia kliknięcia

            // Dodanie przycisku do formularza
            this.Controls.Add(openFileDialogButton);
        }

        private void openFileDialogButton_DragEnter(object sender, DragEventArgs e)
        {
            // Obsługa zdarzenia przeciągania pliku nad przyciskiem
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).ToLowerInvariant() == ".meta")
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void openFileDialogButton_DragDrop(object sender, DragEventArgs e)
        {
            // Obsługa zdarzenia upuszczenia pliku na przycisk
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0];

                // Wyświetlenie nazwy pliku w etykiecie
                selectedFileLabel.Text = "Wybrany plik: " + Path.GetFileName(filePath);
                selectedFilePath = files[0];
            }
        }
        private void InitializeOptions() // Funkcja odpowiadająca za uruchomienie pojawienia się przycisku
        {
            // Inicjalizacja przycisku
            options = new ComboBox();
            options.Items.AddRange(new object[] { "2-osobowe", "4-osobowe", "Bomba" });
            options.SelectedIndex = 0;
            options.Location = new System.Drawing.Point(10, 65);
            options.Size = new System.Drawing.Size(150, 30);
            options.DropDownStyle = ComboBoxStyle.DropDownList; // Ustawienie ComboBox na niemożliwość wpisywania wartości ręcznie

            // Dodanie przycisku do formularza
            this.Controls.Add(options);
            options.SelectedIndexChanged += Options_SelectedIndexChanged;
        }

        private void Options_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedComboBox = options.SelectedIndex;
        }

        private void InitializeOptionsLabel()
        {
            LabelOptions = new Label();
            LabelOptions.Text = "Wybierz typ handlingu:";
            LabelOptions.Location = new System.Drawing.Point(10, 50);
            LabelOptions.AutoSize = true;

            this.Controls.Add(LabelOptions);
        }

        private void InitializeSelectedFileLabel() // Funkcja inicjalizująca etykietę na nazwę wybranego pliku
        {
            selectedFileLabel = new Label();
            selectedFileLabel.Text = "Wybrany plik: ";
            selectedFileLabel.Location = new System.Drawing.Point(120, 18);
            selectedFileLabel.AutoSize = true;

            // Dodanie etykiety do formularza
            this.Controls.Add(selectedFileLabel);
        }

        private void InitializeCheckbox()
        {
            checkbox = new CheckBox();
            checkbox.Text = "Boost Masy";
            checkbox.Location = new System.Drawing.Point(180, 60);
            checkbox.Size = new System.Drawing.Size(85, 30);
            checkbox.CheckedChanged += (sender, e) => selectedcheckbox1 = checkbox.Checked; // Przechwytywanie wartości zaznaczenia checkboxa


            checkbox2 = new CheckBox();
            checkbox2.Text = "Boost Silnika";
            checkbox2.Location = new System.Drawing.Point(270, 60);
            checkbox2.Size = new System.Drawing.Size(90, 30);
            checkbox2.CheckedChanged += (sender, e) => selectedcheckbox2 = checkbox2.Checked; // Przechwytywanie wartości zaznaczenia checkboxa


            this.Controls.Add(checkbox);
            this.Controls.Add(checkbox2);
        }
        private void InitializeSubmit()
        {
            Submit = new Button();
            Submit.Text = "Zmień Handling";
            Submit.Location = new System.Drawing.Point(10, 100);
            Submit.Size = new System.Drawing.Size(100, 30);
            Submit.Click += Submit_Click; // Przypisanie zdarzenia kliknięcia

            this.Controls.Add(Submit);
        }

        private void openFileDialogButton_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "meta files (*.meta)|*.meta";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Pobranie ścieżki wybranego pliku
                    filePath = openFileDialog.FileName;

                    // Wyświetlenie nazwy pliku w etykiecie
                    selectedFileLabel.Text = "Wybrany plik: " + Path.GetFileName(filePath);
                    selectedFilePath = openFileDialog.FileName;
                }
            }
        }
        private void Submit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFileLabel.Text) || selectedFileLabel.Text == "Wybrany plik: ")
            {
                //ShowCustomMessageBox("Nie wybrano pliku!");
                MessageBox.Show(config.TwoOsFmassBoostMass.ToString(CultureInfo.CreateSpecificCulture("en-US")));
                return;
            }
            else
            {
                LoadAndModifyMeta(selectedFilePath, selectedcheckbox1, selectedcheckbox2);
            }
        }
        private void ShowCustomMessageBox(string message)
        {
            // Stworzenie niestandardowego okna dialogowego
            Form customMessageBox = new Form();
            customMessageBox.Size = new Size(180, 150);
            customMessageBox.StartPosition = FormStartPosition.CenterParent;
            customMessageBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            customMessageBox.Text = "Wybierz plik";

            // Tekst komunikatu
            Label label = new Label();
            label.Text = message;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;

            // Przycisk OK
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Dock = DockStyle.Bottom;
            okButton.Click += (sender, e) => customMessageBox.Close();

            // Dodanie kontrolki do okna
            customMessageBox.Controls.Add(label);
            customMessageBox.Controls.Add(okButton);

            // Wyświetlenie okna jako okna dialogowego
            customMessageBox.ShowDialog();
        }
        private void LoadAndModifyMeta(string filePath, bool BoostMass, bool BoostEngine)
        {
            //config variables
            #region 
            string _TwoOsfMass = config.TwoOsfMass.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfInitialDragCoeff = config.TwoOsfInitialDragCoeff.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfDownforceModifier = config.TwoOsfDownforceModifier.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfDriveBiasFront = config.TwoOsfDriveBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsnInitialDriveGears = config.TwoOsnInitialDriveGears.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfInitialDriveForce = config.TwoOsfInitialDriveForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfDriveInertia = config.TwoOsfDriveInertia.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfClutchChangeRateScaleUpShift = config.TwoOsfClutchChangeRateScaleUpShift.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfClutchChangeRateScaleDownShift = config.TwoOsfClutchChangeRateScaleDownShift.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfInitialDriveMaxFlatVel = config.TwoOsfInitialDriveMaxFlatVel.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfBrakeForce = config.TwoOsfBrakeForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfBrakeBiasFront = config.TwoOsfBrakeBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfHandBrakeForce = config.TwoOsfHandBrakeForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfSteeringLock = config.TwoOsfSteeringLock.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfTractionCurveMax = config.TwoOsfTractionCurveMax.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfTractionCurveMin = config.TwoOsfTractionCurveMin.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfLowSpeedTractionLossMult = config.TwoOsfLowSpeedTractionLossMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfCamberStiffnesss = config.TwoOsfCamberStiffnesss.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfTractionBiasFront = config.TwoOsfTractionBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfTractionLossMult = config.TwoOsfTractionLossMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfSuspensionForce = config.TwoOsfSuspensionForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfSuspensionCompDamp = config.TwoOsfSuspensionCompDamp.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfSuspensionReboundDamp = config.TwoOsfSuspensionReboundDamp.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfAntiRollBarForce = config.TwoOsfAntiRollBarForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfAntiRollBarBiasFront = config.TwoOsfAntiRollBarBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfCollisionDamageMult = config.TwoOsfCollisionDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfWeaponDamageMult = config.TwoOsfWeaponDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfDeformationDamageMult = config.TwoOsfDeformationDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfEngineDamageMult = config.TwoOsfEngineDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsFmassBoostMass = config.TwoOsFmassBoostMass.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfInitialDragCoeffBoostEngine = config.TwoOsfInitialDragCoeffBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfInitialDriveForceBoostEngine = config.TwoOsfInitialDriveForceBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfClutchChangeRateScaleUpShiftBoostEngine = config.TwoOsfClutchChangeRateScaleUpShiftBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfClutchChangeRateScaleDownShiftBoostEngine = config.TwoOsfClutchChangeRateScaleDownShiftBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfInitialDriveMaxFlatVelBoostEngine = config.TwoOsfInitialDriveMaxFlatVelBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfSteeringLockBoostEngine = config.TwoOsfSteeringLockBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfTractionCurveMaxBoostEngine = config.TwoOsfTractionCurveMaxBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _TwoOsfTractionCurveMinBoostEngine = config.TwoOsfTractionCurveMinBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfMass = config.FourOsfMass.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfInitialDragCoeff = config.FourOsfInitialDragCoeff.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfDownforceModifier = config.FourOsfDownforceModifier.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfDriveBiasFront = config.FourOsfDriveBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsnInitialDriveGears = config.FourOsnInitialDriveGears.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfInitialDriveForce = config.FourOsfInitialDriveForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfDriveInertia = config.FourOsfDriveInertia.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfClutchChangeRateScaleUpShift = config.FourOsfClutchChangeRateScaleUpShift.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfClutchChangeRateScaleDownShift = config.FourOsfClutchChangeRateScaleDownShift.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfInitialDriveMaxFlatVel = config.FourOsfInitialDriveMaxFlatVel.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfBrakeForce = config.FourOsfBrakeForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfBrakeBiasFront = config.FourOsfBrakeBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfHandBrakeForce = config.FourOsfHandBrakeForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfSteeringLock = config.FourOsfSteeringLock.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfTractionCurveMax = config.FourOsfTractionCurveMax.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfTractionCurveMin = config.FourOsfTractionCurveMin.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfLowSpeedTractionLossMult = config.FourOsfLowSpeedTractionLossMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfCamberStiffnesss = config.FourOsfCamberStiffnesss.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfTractionBiasFront = config.FourOsfTractionBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfTractionLossMult = config.FourOsfTractionLossMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfSuspensionForce = config.FourOsfSuspensionForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfSuspensionCompDamp = config.FourOsfSuspensionCompDamp.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfSuspensionReboundDamp = config.FourOsfSuspensionReboundDamp.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfAntiRollBarForce = config.FourOsfAntiRollBarForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfAntiRollBarBiasFront = config.FourOsfAntiRollBarBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfCollisionDamageMult = config.FourOsfCollisionDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfWeaponDamageMult = config.FourOsfWeaponDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfDeformationDamageMult = config.FourOsfDeformationDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfEngineDamageMult = config.FourOsfEngineDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsFmassBoostMass = config.FourOsFmassBoostMass.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfInitialDragCoeffBoostEngine = config.FourOsfInitialDragCoeffBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfInitialDriveForceBoostEngine = config.FourOsfInitialDriveForceBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfClutchChangeRateScaleUpShiftBoostEngine = config.FourOsfClutchChangeRateScaleUpShiftBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfClutchChangeRateScaleDownShiftBoostEngine = config.FourOsfClutchChangeRateScaleDownShiftBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfInitialDriveMaxFlatVelBoostEngine = config.FourOsfInitialDriveMaxFlatVelBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfSteeringLockBoostEngine = config.FourOsfSteeringLockBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfTractionCurveMaxBoostEngine = config.FourOsfTractionCurveMaxBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _FourOsfTractionCurveMinBoostEngine = config.FourOsfTractionCurveMinBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafMass = config.BombafMass.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafInitialDragCoeff = config.BombafInitialDragCoeff.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafDownforceModifier = config.BombafDownforceModifier.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafDriveBiasFront = config.BombafDriveBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombanInitialDriveGears = config.BombanInitialDriveGears.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafInitialDriveForce = config.BombafInitialDriveForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafDriveInertia = config.BombafDriveInertia.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafClutchChangeRateScaleUpShift = config.BombafClutchChangeRateScaleUpShift.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafClutchChangeRateScaleDownShift = config.BombafClutchChangeRateScaleDownShift.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafInitialDriveMaxFlatVel = config.BombafInitialDriveMaxFlatVel.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafBrakeForce = config.BombafBrakeForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafBrakeBiasFront = config.BombafBrakeBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafHandBrakeForce = config.BombafHandBrakeForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafSteeringLock = config.BombafSteeringLock.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafTractionCurveMax = config.BombafTractionCurveMax.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafTractionCurveMin = config.BombafTractionCurveMin.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafLowSpeedTractionLossMult = config.BombafLowSpeedTractionLossMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafCamberStiffnesss = config.BombafCamberStiffnesss.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafTractionBiasFront = config.BombafTractionBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafTractionLossMult = config.BombafTractionLossMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafSuspensionForce = config.BombafSuspensionForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafSuspensionCompDamp = config.BombafSuspensionCompDamp.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafSuspensionReboundDamp = config.BombafSuspensionReboundDamp.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafAntiRollBarForce = config.BombafAntiRollBarForce.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafAntiRollBarBiasFront = config.BombafAntiRollBarBiasFront.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafCollisionDamageMult = config.BombafCollisionDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafWeaponDamageMult = config.BombafWeaponDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafDeformationDamageMult = config.BombafDeformationDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafEngineDamageMult = config.BombafEngineDamageMult.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombaFmassBoostMass = config.BombaFmassBoostMass.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafInitialDragCoeffBoostEngine = config.BombafInitialDragCoeffBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafInitialDriveForceBoostEngine = config.BombafInitialDriveForceBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafClutchChangeRateScaleUpShiftBoostEngine = config.BombafClutchChangeRateScaleUpShiftBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafClutchChangeRateScaleDownShiftBoostEngine = config.BombafClutchChangeRateScaleDownShiftBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafInitialDriveMaxFlatVelBoostEngine = config.BombafInitialDriveMaxFlatVelBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafSteeringLockBoostEngine = config.BombafSteeringLockBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafTractionCurveMaxBoostEngine = config.BombafTractionCurveMaxBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            string _BombafTractionCurveMinBoostEngine = config.BombafTractionCurveMinBoostEngine.ToString(CultureInfo.CreateSpecificCulture("en-US"));
            #endregion 

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filePath);
                int option = selectedComboBox;
                switch (option)
                {
                    case 0:
                        if (!BoostMass)
                        {
                            ModifyXMLValues(doc, "fMass", _TwoOsfMass);
                        }
                        else
                        {
                            ModifyXMLValues(doc, "fMass", _TwoOsFmassBoostMass);
                        }
                        if (!BoostEngine)
                        {
                            ModifyXMLValues(doc, "fInitialDragCoeff", _TwoOsfInitialDragCoeff);
                            ModifyXMLValues(doc, "fInitialDriveForce", _TwoOsfInitialDriveForce);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleUpShift", _TwoOsfClutchChangeRateScaleUpShift);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleDownShift", _TwoOsfClutchChangeRateScaleDownShift);
                            ModifyXMLValues(doc, "fInitialDriveMaxFlatVel", _TwoOsfInitialDriveMaxFlatVel);
                            ModifyXMLValues(doc, "fSteeringLock", _TwoOsfSteeringLock);
                            ModifyXMLValues(doc, "fTractionCurveMax", _TwoOsfTractionCurveMax);
                            ModifyXMLValues(doc, "fTractionCurveMin", _TwoOsfTractionCurveMin);
                        }
                        else
                        {
                            ModifyXMLValues(doc, "fInitialDragCoeff", _TwoOsfInitialDragCoeffBoostEngine);
                            ModifyXMLValues(doc, "fInitialDriveForce", _TwoOsfInitialDriveForceBoostEngine);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleUpShift", _TwoOsfClutchChangeRateScaleUpShiftBoostEngine);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleDownShift", _TwoOsfClutchChangeRateScaleDownShiftBoostEngine);
                            ModifyXMLValues(doc, "fInitialDriveMaxFlatVel", _TwoOsfInitialDriveMaxFlatVelBoostEngine);
                            ModifyXMLValues(doc, "fSteeringLock", _TwoOsfSteeringLockBoostEngine);
                            ModifyXMLValues(doc, "fTractionCurveMax", _TwoOsfTractionCurveMaxBoostEngine);
                            ModifyXMLValues(doc, "fTractionCurveMin", _TwoOsfTractionCurveMinBoostEngine);
                        }
                        ModifyXMLValues(doc, "fDownforceModifier", _TwoOsfDownforceModifier);
                        ModifyXMLValues(doc, "fDriveBiasFront", _TwoOsfDriveBiasFront);
                        ModifyXMLValues(doc, "nInitialDriveGears", _TwoOsnInitialDriveGears);
                        ModifyXMLValues(doc, "fDriveInertia", _TwoOsfDriveInertia);
                        ModifyXMLValues(doc, "fBrakeForce", _TwoOsfBrakeForce);
                        ModifyXMLValues(doc, "fBrakeBiasFront", _TwoOsfBrakeBiasFront);
                        ModifyXMLValues(doc, "fHandBrakeForce", _TwoOsfHandBrakeForce);
                        ModifyXMLValues(doc, "fLowSpeedTractionLossMult", _TwoOsfLowSpeedTractionLossMult);
                        ModifyXMLValues(doc, "fCamberStiffnesss", _TwoOsfCamberStiffnesss);
                        ModifyXMLValues(doc, "fTractionBiasFront", _TwoOsfTractionBiasFront);
                        ModifyXMLValues(doc, "fTractionLossMult", _TwoOsfTractionLossMult);
                        ModifyXMLValues(doc, "fSuspensionForce", _TwoOsfSuspensionForce);
                        ModifyXMLValues(doc, "fSuspensionCompDamp", _TwoOsfSuspensionCompDamp);
                        ModifyXMLValues(doc, "fSuspensionReboundDamp", _TwoOsfSuspensionReboundDamp);
                        ModifyXMLValues(doc, "fAntiRollBarForce", _TwoOsfAntiRollBarForce);
                        ModifyXMLValues(doc, "fAntiRollBarBiasFront", _TwoOsfAntiRollBarBiasFront);
                        ModifyXMLValues(doc, "fCollisionDamageMult", _TwoOsfCollisionDamageMult);
                        ModifyXMLValues(doc, "fWeaponDamageMult", _TwoOsfWeaponDamageMult);
                        ModifyXMLValues(doc, "fDeformationDamageMult", _TwoOsfDeformationDamageMult);
                        ModifyXMLValues(doc, "fEngineDamageMult", _TwoOsfEngineDamageMult);
                        break;
                    case 1:
                        if (!BoostMass)
                        {
                            ModifyXMLValues(doc, "fMass", _FourOsfMass);
                        }
                        else
                        {
                            ModifyXMLValues(doc, "fMass", _FourOsFmassBoostMass);
                        }
                        if (!BoostEngine)
                        {
                            ModifyXMLValues(doc, "fInitialDragCoeff", _FourOsfInitialDragCoeff);
                            ModifyXMLValues(doc, "fInitialDriveForce", _FourOsfInitialDriveForce);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleUpShift", _FourOsfClutchChangeRateScaleUpShift);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleDownShift", _FourOsfClutchChangeRateScaleDownShift);
                            ModifyXMLValues(doc, "fInitialDriveMaxFlatVel", _FourOsfInitialDriveMaxFlatVel);
                            ModifyXMLValues(doc, "fSteeringLock", _FourOsfSteeringLock);
                            ModifyXMLValues(doc, "fTractionCurveMax", _FourOsfTractionCurveMax);
                            ModifyXMLValues(doc, "fTractionCurveMin", _FourOsfTractionCurveMin);
                        }
                        else
                        {
                            ModifyXMLValues(doc, "fInitialDragCoeff", _FourOsfInitialDragCoeffBoostEngine);
                            ModifyXMLValues(doc, "fInitialDriveForce", _FourOsfInitialDriveForceBoostEngine);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleUpShift", _FourOsfClutchChangeRateScaleUpShiftBoostEngine);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleDownShift", _FourOsfClutchChangeRateScaleDownShiftBoostEngine);
                            ModifyXMLValues(doc, "fInitialDriveMaxFlatVel", _FourOsfInitialDriveMaxFlatVelBoostEngine);
                            ModifyXMLValues(doc, "fSteeringLock", _FourOsfSteeringLockBoostEngine);
                            ModifyXMLValues(doc, "fTractionCurveMax", _FourOsfTractionCurveMaxBoostEngine);
                            ModifyXMLValues(doc, "fTractionCurveMin", _FourOsfTractionCurveMinBoostEngine);
                        }
                        ModifyXMLValues(doc, "fDownforceModifier", _FourOsfDownforceModifier);
                        ModifyXMLValues(doc, "fDriveBiasFront", _FourOsfDriveBiasFront);
                        ModifyXMLValues(doc, "nInitialDriveGears", _FourOsnInitialDriveGears);
                        ModifyXMLValues(doc, "fDriveInertia", _FourOsfDriveInertia);
                        ModifyXMLValues(doc, "fBrakeForce", _FourOsfBrakeForce);
                        ModifyXMLValues(doc, "fBrakeBiasFront", _FourOsfBrakeBiasFront);
                        ModifyXMLValues(doc, "fHandBrakeForce", _FourOsfHandBrakeForce);
                        ModifyXMLValues(doc, "fLowSpeedTractionLossMult", _FourOsfLowSpeedTractionLossMult);
                        ModifyXMLValues(doc, "fCamberStiffnesss", _FourOsfCamberStiffnesss);
                        ModifyXMLValues(doc, "fTractionBiasFront", _FourOsfTractionBiasFront);
                        ModifyXMLValues(doc, "fTractionLossMult", _FourOsfTractionLossMult);
                        ModifyXMLValues(doc, "fSuspensionForce", _FourOsfSuspensionForce);
                        ModifyXMLValues(doc, "fSuspensionCompDamp", _FourOsfSuspensionCompDamp);
                        ModifyXMLValues(doc, "fSuspensionReboundDamp", _FourOsfSuspensionReboundDamp);
                        ModifyXMLValues(doc, "fAntiRollBarForce", _FourOsfAntiRollBarForce);
                        ModifyXMLValues(doc, "fAntiRollBarBiasFront", _FourOsfAntiRollBarBiasFront);
                        ModifyXMLValues(doc, "fCollisionDamageMult", _FourOsfCollisionDamageMult);
                        ModifyXMLValues(doc, "fWeaponDamageMult", _FourOsfWeaponDamageMult);
                        ModifyXMLValues(doc, "fDeformationDamageMult", _FourOsfDeformationDamageMult);
                        ModifyXMLValues(doc, "fEngineDamageMult", _FourOsfEngineDamageMult);
                        break;
                    case 2:
                        if (!BoostMass)
                        {
                            ModifyXMLValues(doc, "fMass", _BombafMass);
                        }
                        else
                        {
                            ModifyXMLValues(doc, "fMass", _BombaFmassBoostMass);
                        }
                        if (!BoostEngine)
                        {
                            ModifyXMLValues(doc, "fInitialDragCoeff", _BombafInitialDragCoeff);
                            ModifyXMLValues(doc, "fInitialDriveForce", _BombafInitialDriveForce);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleUpShift", _BombafClutchChangeRateScaleUpShift);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleDownShift", _BombafClutchChangeRateScaleDownShift);
                            ModifyXMLValues(doc, "fInitialDriveMaxFlatVel", _BombafInitialDriveMaxFlatVel);
                            ModifyXMLValues(doc, "fSteeringLock", _BombafSteeringLock);
                            ModifyXMLValues(doc, "fTractionCurveMax", _BombafTractionCurveMax);
                            ModifyXMLValues(doc, "fTractionCurveMin", _BombafTractionCurveMin);
                        }
                        else
                        {
                            ModifyXMLValues(doc, "fInitialDragCoeff", _BombafInitialDragCoeffBoostEngine);
                            ModifyXMLValues(doc, "fInitialDriveForce", _BombafInitialDriveForceBoostEngine);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleUpShift", _BombafClutchChangeRateScaleUpShiftBoostEngine);
                            ModifyXMLValues(doc, "fClutchChangeRateScaleDownShift", _BombafClutchChangeRateScaleDownShiftBoostEngine);
                            ModifyXMLValues(doc, "fInitialDriveMaxFlatVel", _BombafInitialDriveMaxFlatVelBoostEngine);
                            ModifyXMLValues(doc, "fSteeringLock", _BombafSteeringLockBoostEngine);
                            ModifyXMLValues(doc, "fTractionCurveMax", _BombafTractionCurveMaxBoostEngine);
                            ModifyXMLValues(doc, "fTractionCurveMin", _BombafTractionCurveMinBoostEngine);
                        }
                        ModifyXMLValues(doc, "fDownforceModifier", _BombafDownforceModifier);
                        ModifyXMLValues(doc, "fDriveBiasFront", _BombafDriveBiasFront);
                        ModifyXMLValues(doc, "nInitialDriveGears", _BombanInitialDriveGears);
                        ModifyXMLValues(doc, "fDriveInertia", _BombafDriveInertia);
                        ModifyXMLValues(doc, "fBrakeForce", _BombafBrakeForce);
                        ModifyXMLValues(doc, "fBrakeBiasFront", _BombafBrakeBiasFront);
                        ModifyXMLValues(doc, "fHandBrakeForce", _BombafHandBrakeForce);
                        ModifyXMLValues(doc, "fLowSpeedTractionLossMult", _BombafLowSpeedTractionLossMult);
                        ModifyXMLValues(doc, "fCamberStiffnesss", _BombafCamberStiffnesss);
                        ModifyXMLValues(doc, "fTractionBiasFront", _BombafTractionBiasFront);
                        ModifyXMLValues(doc, "fTractionLossMult", _BombafTractionLossMult);
                        ModifyXMLValues(doc, "fSuspensionForce", _BombafSuspensionForce);
                        ModifyXMLValues(doc, "fSuspensionCompDamp", _BombafSuspensionCompDamp);
                        ModifyXMLValues(doc, "fSuspensionReboundDamp", _BombafSuspensionReboundDamp);
                        ModifyXMLValues(doc, "fAntiRollBarForce", _BombafAntiRollBarForce);
                        ModifyXMLValues(doc, "fAntiRollBarBiasFront", _BombafAntiRollBarBiasFront);
                        ModifyXMLValues(doc, "fCollisionDamageMult", _BombafCollisionDamageMult);
                        ModifyXMLValues(doc, "fWeaponDamageMult", _BombafWeaponDamageMult);
                        ModifyXMLValues(doc, "fDeformationDamageMult", _BombafDeformationDamageMult);
                        ModifyXMLValues(doc, "fEngineDamageMult", _BombafEngineDamageMult);
                        break;
                    default:
                        MessageBox.Show("Błąd, nie wybrano poprawnej opcji");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania lub modyfikowania pliku XML: {ex.Message}");
            }

            string newFilePath = Path.ChangeExtension(filePath, ".meta");
            doc.Save(newFilePath);
            ShowCustomMessageBox2($"Zmodyfikowano plik: {Path.GetFileName(filePath)}");
        }
        private void ModifyXMLValues(XmlDocument doc, string nodeName, string newValue)
        {
            XmlNodeList nodes = doc.SelectNodes($"//{nodeName}[@value]");
            if (nodes.Count == 0)
            {
                // Tworzymy nowy węzeł
                XmlNode newNode = doc.CreateElement(nodeName);
                XmlAttribute attr = doc.CreateAttribute("value");
                attr.Value = newValue;
                newNode.Attributes.Append(attr);

                // Dodajemy nowy węzeł do dokumentu
                XmlNode rootNode = doc.SelectSingleNode("CHandlingDataMgr/HandlingData/Item");
                rootNode.AppendChild(newNode);
            }
            else
            {
                // Modyfikujemy wartość istniejącego węzła
                foreach (XmlNode node in nodes)
                {
                    node.Attributes["value"].Value = newValue;
                }
            }
        }

        private void ShowCustomMessageBox2(string message)
        {
            // Stworzenie niestandardowego okna dialogowego
            Form customMessageBox = new Form();
            customMessageBox.Size = new Size(180, 150);
            customMessageBox.StartPosition = FormStartPosition.CenterParent;
            customMessageBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            customMessageBox.Text = "Modyfikacja pliku";

            // Tekst komunikatu
            Label label = new Label();
            label.Text = message;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;

            // Przycisk OK
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Dock = DockStyle.Bottom;
            okButton.Click += (sender, e) => customMessageBox.Close();

            // Dodanie kontrolki do okna
            customMessageBox.Controls.Add(label);
            customMessageBox.Controls.Add(okButton);

            // Wyświetlenie okna jako okna dialogowego
            customMessageBox.ShowDialog();
        }
        private void MainForm_Load(object sender, EventArgs e)
        { 

        }
    }
}