namespace SudokuCracker
{
    /// <summary>
    /// �i���v����ǃV�X�e��
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
            // backspace, delete��L����
            if (e.KeyChar == '\b') return;
            // �����ꂽ�L�[�� 1�`9�łȂ��ꍇ�A�C�x���g���L�����Z��
            if (e.KeyChar < '1' || '9' < e.KeyChar) e.Handled = true;
        }
    }
}