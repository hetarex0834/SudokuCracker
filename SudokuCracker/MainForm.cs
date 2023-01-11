namespace SudokuCracker
{
    /// <summary>
    /// ナンプレ解読システム
    /// </summary>
    public partial class MainForm : Form
    {
        // 一列のマス目の個数
        private static readonly int Number = 9;
        // マス目の個数
        private static readonly int Max = Number * Number;
        // マス目
        private readonly TextBox[,] txtSquares = new TextBox[Number, Number];
        // マス目の数字
        private readonly int[] sqVals = new int[Number * Number];
        // マス目のX座標の初期値
        private readonly int StartX = 160;
        // マス目のY座標の初期値
        private readonly int StartY = 80;
        // マス目の配置間隔
        private readonly int SqSpace = 60;
        // マス目のサイズ
        private readonly int SqSize = 55;

        /// <summary>
        /// コンポーネントを設定
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // マス目を作成
            for (var i = 0; i < Number; i++)
            {
                for (var j = 0; j < Number; j++)
                {
                    txtSquares[i, j] = new TextBox()
                    {
                        ImeMode = ImeMode.Disable,
                        MaxLength = 1,
                        ShortcutsEnabled = false,
                        TabIndex = i * Number + j + 1,
                        Location = new Point(StartX + SqSpace * j, StartY + SqSpace * i),
                        Size = new Size(SqSize, SqSize),
                        Font = new Font("Yu Gothic UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                        TextAlign = HorizontalAlignment.Center,
                    };

                    // 3×3マスごとに背景色を設定
                    if (i < 3 || 5 < i)
                    {
                        if (3 <= j && j <= 5) txtSquares[i, j].BackColor = Color.LightGray;
                    }
                    else
                    {
                        if (j < 3 || 5 < j) txtSquares[i, j].BackColor = Color.LightGray;
                    }

                    txtSquares[i, j].KeyPress += TxtSquares_KeyPress;
                    Controls.Add(txtSquares[i, j]);
                }
            }
        }

        /// <summary>
        /// 入力可能なキーを制限
        /// </summary>
        private void TxtSquares_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b') return;
            if (e.KeyChar < '1' || '9' < e.KeyChar) e.Handled = true;
        }

        /// <summary>
        /// 解読ボタンクリックイベント
        /// </summary>
        private void BtnClack_Click(object sender, EventArgs e)
        {
            bool flg = false; // 解読完了フラグ
            SetSquareValue();
            Solve(0, ref flg);
        }

        /// <summary>
        /// マス目を初期化
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            foreach (var t in txtSquares) t.Text = "";
        }

        /// <summary>
        /// マス目の数字を設定
        /// </summary>
        private void SetSquareValue()
        {
            for (var i = 0; i < Number; i++)
            {
                for (var j = 0; j < Number; j++)
                {
                    sqVals[i * Number + j] = (txtSquares[i, j].Text != "") ? int.Parse(txtSquares[i, j].Text) : 0;
                }
            }
        }

        /// <summary>
        /// ナンプレ解読
        /// </summary>
        /// <param name="x">マス目の添字</param>
        /// <param name="flg">解読完了フラグ</param>
        private void Solve(int x, ref bool flg)
        {
            if (x > Max - 1)
            {
                PrintSquare();
                flg = true;
            }
            else
            {
                if (sqVals[x] != 0) Solve(x + 1, ref flg);
                else
                {
                    for (var i = 1; i <= 9; i++)
                    {
                        if (flg) break;
                        if (Check(i, x))
                        {
                            sqVals[x] = i;
                            Solve(x + 1, ref flg);
                            sqVals[x] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解読結果をマス目に出力
        /// </summary>
        private void PrintSquare()
        {
            for (var i = 0; i < Number; i++)
            {
                for (var j = 0; j < Number; j++)
                {
                    txtSquares[i, j].Text = $"{sqVals[i * Number + j]}";
                }
            }
        }

        /// <summary>
        /// 数字の重複チェック
        /// </summary>
        /// <param name="n">チェック対象の数字</param>
        /// <param name="x">チェック対象のマス目の添字</param>
        /// <returns>真偽値</returns>
        private bool Check(int n, int x)
        {
            var rowTop = x / Number * Number;
            var columnTop = x % Number;
            var frameTop = x - x / Number * Number % 27 - x % 3;

            for (var i = 0; i < Number; i++)
            {
                if (sqVals[rowTop + i] == n) return false;
                if (sqVals[columnTop + i * Number] == n) return false;
            }

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (sqVals[frameTop + Number * i + j] == n) return false;
                }
            }

            return true;
        }
    }
}