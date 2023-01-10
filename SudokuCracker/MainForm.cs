namespace SudokuCracker
{
    /// <summary>
    /// ナンプレ解読システム
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtTest.KeyPress += TextBox_KeyPress;
        }

        private void TextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // backspace, deleteを有効化
            if (e.KeyChar == '\b') return;
            // 押されたキーが 1～9でない場合、イベントをキャンセル
            if (e.KeyChar < '1' || '9' < e.KeyChar) e.Handled = true;
        }
    }
}