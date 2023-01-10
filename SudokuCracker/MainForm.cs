namespace SudokuCracker
{
    /// <summary>
    /// �i���v����ǃV�X�e��
    /// </summary>
    public partial class MainForm : Form
    {
        // �s��
        private static readonly int Row = 9;
        // ��
        private static readonly int Column = 9;
        // �e�L�X�g�{�b�N�X
        private readonly TextBox[,] txts = new TextBox[Row, Column];

        public MainForm()
        {
            InitializeComponent();

            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    txts[i, j] = new TextBox()
                    {
                        ImeMode = ImeMode.Disable,
                        MaxLength = 1,
                        ShortcutsEnabled = false,
                        TabIndex = i * Row + j + 1,
                        Location = new Point(160 + 60 * j, 100 + 60 * i),
                        Size = new Size(55, 55),
                        Font = new Font("Yu Gothic UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                        TextAlign = HorizontalAlignment.Center,
                    };
                    if (i < 3 || 5 < i)
                    {
                        if (3 <= j && j <= 5) txts[i, j].BackColor = Color.LightGray;
                    }
                    else
                    {
                        if (j < 3 || 5 < j) txts[i, j].BackColor = Color.LightGray;
                    }
                    txts[i, j].KeyPress += TextBox_KeyPress;
                    this.Controls.Add(txts[i, j]);
                }
            }
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