namespace SudokuCracker
{
    /// <summary>
    /// �i���v����ǃV�X�e��
    /// </summary>
    public partial class MainForm : Form
    {
        // �}�X�̋�؂�
        private static readonly int Frame = 3;
        // ���̃}�X�̌�
        private static readonly int Num = Frame * 3;
        // �}�X�̌�
        private static readonly int Max = Num * Num;
        // �}�X��X���W�̏����l
        private static readonly int StartX = 160;
        // �}�X��Y���W�̏����l
        private static readonly int StartY = 80;
        // �}�X�̔z�u�Ԋu
        private static readonly int SqSpace = 60;
        // �}�X�̃T�C�Y
        private static readonly int SqSize = 55;
        // �}�X
        private readonly TextBox[,] txtSquares = new TextBox[Num, Num];
        // �}�X�̐���
        private readonly int[] sqVals = new int[Max];

        /// <summary>
        /// �R���|�[�l���g��ݒ�
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // �}�X���쐬
            for (var i = 0; i < Num; i++)
            {
                for (var j = 0; j < Num; j++)
                {
                    txtSquares[i, j] = new TextBox()
                    {
                        Text = "",
                        ImeMode = ImeMode.Disable,
                        MaxLength = 1,
                        ShortcutsEnabled = false,
                        TabIndex = i * Num + j + 1,
                        Location = new Point(StartX + SqSpace * j, StartY + SqSpace * i),
                        Size = new Size(SqSize, SqSize),
                        Font = new Font("Yu Gothic UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                        TextAlign = HorizontalAlignment.Center,
                    };

                    // 3�~3�}�X���Ƃɔw�i�F��ݒ�
                    if (i < Frame || Frame * 2 - 1 < i)
                    {
                        if (Frame <= j && j <= Frame * 2 - 1) txtSquares[i, j].BackColor = Color.LightGray;
                    }
                    else
                    {
                        if (j < Frame || Frame * 2 - 1 < j) txtSquares[i, j].BackColor = Color.LightGray;
                    }

                    txtSquares[i, j].KeyPress += TxtSquares_KeyPress;
                    Controls.Add(txtSquares[i, j]);
                }
            }
        }

        /// <summary>
        /// ���͉\�ȃL�[�𐧌�
        /// </summary>
        private void TxtSquares_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b') return;
            if (e.KeyChar < '1' || '9' < e.KeyChar) e.Handled = true;
        }

        /// <summary>
        /// ��ǃ{�^���N���b�N�C�x���g
        /// </summary>
        private void BtnClack_Click(object sender, EventArgs e)
        {
            // �^�C���A�E�g�p�I�u�W�F�N�g
            using var cts = new CancellationTokenSource();
            try
            {
                // ��Ǌ����t���O
                var flg = false;
                // �^�C���A�E�g��ݒ�
                cts.CancelAfter(5000);
                // ��ǊJ�n
                SetSquareValue();
                SquareSearch(0, ref flg, cts.Token);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("�^�C���A�E�g���܂����B", "�^�C���A�E�g", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// �}�X��������
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            foreach (var t in txtSquares) t.Text = "";
        }

        /// <summary>
        /// �}�X�̐�����ݒ�
        /// </summary>
        private void SetSquareValue()
        {
            for (var i = 0; i < Num; i++)
            {
                for (var j = 0; j < Num; j++)
                {
                    sqVals[i * Num + j] = (txtSquares[i, j].Text != "") ? int.Parse(txtSquares[i, j].Text) : 0;
                }
            }
        }

        /// <summary>
        /// �}�X�̒T��
        /// </summary>
        /// <param name="x">�}�X�̓Y��</param>
        /// <param name="flg">��Ǌ����t���O</param>
        /// <param name="ct">�^�C���A�E�g�p�I�u�W�F�N�g</param>
        private void SquareSearch(int x, ref bool flg, CancellationToken ct)
        {
            // �^�C���A�E�g�̏ꍇ�A��O�����𓊂���
            ct.ThrowIfCancellationRequested();

            if (x > Max - 1)
            {
                // ��ǌ��ʂ��o��
                PrintSquare();
                flg = true;
            }
            else
            {
                // �}�X���󔒂łȂ��ꍇ�A���̒T���ֈڍs
                if (sqVals[x] != 0) SquareSearch(x + 1, ref flg, ct);
                else
                {
                    // 1~9�̐��������Ƀ}�X�ɓ����
                    for (var n = 1; n <= Num; n++)
                    {
                        if (flg) break;
                        if (Check(n, x))
                        {
                            sqVals[x] = n;
                            SquareSearch(x + 1, ref flg, ct); // ���̒T��
                            sqVals[x] = 0; // �}�X�̏�����
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ��ǌ��ʂ��}�X�ɏo��
        /// </summary>
        private void PrintSquare()
        {
            for (var i = 0; i < Num; i++)
            {
                for (var j = 0; j < Num; j++)
                {
                    txtSquares[i, j].Text = $"{sqVals[i * Num + j]}";
                }
            }
        }

        /// <summary>
        /// �����̏d���`�F�b�N
        /// </summary>
        /// <param name="n">�`�F�b�N�Ώۂ̐���</param>
        /// <param name="x">�`�F�b�N�Ώۂ̃}�X�̓Y��</param>
        /// <returns>�^�U�l</returns>
        private bool Check(int n, int x)
        {
            // ��1�s�̍��[�̃}�X�̓Y��
            var rowTop = x / Num * Num;
            // �c1��̏�[�̃}�X�̓Y��
            var columnTop = x % Num;
            // 3�~3�}�X�̍���[�̃}�X�̓Y��
            var frameTop = x - x / Num * Num % (Num * Frame) - x % Frame;

            // ��1�s�E�c1��̏d���`�F�b�N
            for (var i = 0; i < Num; i++)
            {
                if (sqVals[rowTop + i] == n) return false;
                if (sqVals[columnTop + i * Num] == n) return false;
            }
            // 3�~3�}�X�̏d���`�F�b�N
            for (var i = 0; i < Frame; i++)
            {
                for (var j = 0; j < Frame; j++)
                {
                    if (sqVals[frameTop + i * Num + j] == n) return false;
                }
            }

            return true;
        }
    }
}