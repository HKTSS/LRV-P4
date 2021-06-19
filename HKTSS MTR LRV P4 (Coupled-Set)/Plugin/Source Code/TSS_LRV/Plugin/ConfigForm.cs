using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plugin {
    public partial class ConfigForm : Form {
        public ConfigForm() {
            InitializeComponent();
            if (Plugin.language.ToLowerInvariant().StartsWith("zh")) {
                TranslateForm2Chinese();
            }
        }

        internal void TranslateForm2Chinese() {
            Text = "列車設定";
            TrainCarNumberLabel.Text = "車卡編號";
            SafetySystemLabel.Text = "安全系統";
            MiscLabel.Text = "其他";
            FirstCarLabel.Text = "第一卡:";
            SecondCarLabel.Text = "第二卡:";
            DoorLockCheckBox.Text = "列車未停定的情況下，鎖住車門";
            ApplyBrakeCheckBox.Text = "車門開啟時，鎖住牽引動力";
            iSPSDoorLockEnabled.Text = "駛入月台後，如車門未開啟時，抑制牽引動力";
            CrashCheckBox.Text = "開啟撞車效果";
            MTRBeeping.Text = "使用港鐵化關門提示聲";
            RevAtStation.Text = "允許進站後倒車";
            /* Change the font size of the TrainStatusLabel */
            TrainStatusLabel.Font = new Font("Arial", 8.75F);
            TrainStatusLabel.Text = "列車狀態指示牌: ";
            /* Have to move down the TrainStatusLabel 2 pixels down when in chinese to make it looks right. */
            TrainStatusLabel.Location = new Point(TrainStatusLabel.Location.X, TrainStatusLabel.Location.Y + 2);
            TrainStatusBox.Items[0] = "沒有";
            TrainStatusBox.Items[1] = "NOT TO GO (九鐵)";
            TrainStatusBox.Items[2] = "NOT TO GO (港鐵)";
            TrainStatusBox.Items[3] = "SCOTCH BLOCK (港鐵)";
            ApplyChanges.Text = "確定";
        }

        internal static void LaunchForm() {
            if (Application.OpenForms.OfType<ConfigForm>().Any()) {
                Application.OpenForms.OfType<ConfigForm>().First().BringToFront();
            } else {
                Task.Run(() => Application.Run(new ConfigForm()));
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e) {
            DoorLockCheckBox.Checked = Plugin.doorlockEnabled;
            ApplyBrakeCheckBox.Checked = Plugin.doorApplyBrake;
            iSPSDoorLockEnabled.Checked = Plugin.iSPSEnabled;
            CrashCheckBox.Checked = Plugin.crashEnabled;
            MTRBeeping.Checked = Plugin.mtrBeeping;
            RevAtStation.Checked = Plugin.AllowReverseAtStation;
            TrainStatusBox.SelectedIndex = Plugin.trainStatus;
            if (Plugin.LRVGeneration == Misc.LRVType.P1 || Plugin.LRVGeneration == Misc.LRVType.P1R) {
                CarNum1Box.Minimum = 1001;
                CarNum1Box.Maximum = 1090;
                CarNum2Box.Minimum = 1001;
                CarNum2Box.Maximum = 1090;
            } else if (Plugin.LRVGeneration == Misc.LRVType.P3) {
                CarNum1Box.Minimum = 1091;
                CarNum1Box.Maximum = 1110;
                CarNum2Box.Minimum = 1091;
                CarNum2Box.Maximum = 1110;
            } else if (Plugin.LRVGeneration == Misc.LRVType.P4) {
                CarNum1Box.Minimum = 1111;
                CarNum1Box.Maximum = 1132;
                CarNum2Box.Minimum = 1111;
                CarNum2Box.Maximum = 1132;
            } else if (Plugin.LRVGeneration == Misc.LRVType.P5) {
                CarNum1Box.Minimum = 1133;
                CarNum1Box.Maximum = 1162;
                CarNum2Box.Minimum = 1211;
                CarNum2Box.Maximum = 1220;
            }
            CarNum1Box.Value = Plugin.carNum1;
            CarNum2Box.Value = Plugin.carNum2;
            if (Plugin.totalCar < 2) {
                CarNum2Box.Enabled = false;
            }
        }

        private void CarNumFilter(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-')) e.Handled = true;
        }

        private void ApplyChanges_Click(object sender, EventArgs e) {
            this.Close();
            this.Invalidate();
            Plugin.trainStatus = TrainStatusBox.SelectedIndex;
            Plugin.carNum1 = Convert.ToInt32(CarNum1Box.Value);
            Plugin.carNum2 = Convert.ToInt32(CarNum2Box.Value);
            Plugin.iSPSEnabled = iSPSDoorLockEnabled.Checked;
            Plugin.doorlockEnabled = DoorLockCheckBox.Checked;
            Plugin.doorApplyBrake = ApplyBrakeCheckBox.Checked;
            Plugin.crashEnabled = CrashCheckBox.Checked;
            Plugin.mtrBeeping = MTRBeeping.Checked;
            Plugin.AllowReverseAtStation = RevAtStation.Checked;
            Plugin.ChangeTrainNumber(1, Convert.ToInt32(Misc.CarNumPanel((int) CarNum1Box.Value)));
            Plugin.ChangeTrainNumber(2, Convert.ToInt32(Misc.CarNumPanel((int) CarNum2Box.Value)));
            ConfigManager.WriteConfig("CarNum", CarNum1Box.Value + "," + CarNum2Box.Value);
            ConfigManager.WriteConfig("DoorLock", DoorLockCheckBox.Checked.ToString().ToLowerInvariant());
            ConfigManager.WriteConfig("DoorBrake", ApplyBrakeCheckBox.Checked.ToString().ToLowerInvariant());
            ConfigManager.WriteConfig("iSPSdoorlock", iSPSDoorLockEnabled.Checked.ToString().ToLowerInvariant());
            ConfigManager.WriteConfig("Crash", CrashCheckBox.Checked.ToString().ToLowerInvariant());
			ConfigManager.WriteConfig("MTRbeep", MTRBeeping.Checked.ToString().ToLowerInvariant());
            ConfigManager.WriteConfig("RevAtStation", RevAtStation.Checked.ToString().ToLowerInvariant());
            ConfigManager.WriteConfig("TrainStatus", TrainStatusBox.SelectedIndex.ToString());
        }
	}
}
