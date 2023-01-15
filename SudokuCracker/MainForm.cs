namespace SudokuCracker
{
    /// <summary>
    /// ナンプレ解読システム
    /// </summary>
    public partial class MainForm : Form
    {
        // マスの区切り
        private static readonly int Frame = 3;
        // 一列のマスの個数
        private static readonly int Num = Frame * 3;
        // マスの個数
        private static readonly int Max = Num * Num;
        // マスのX座標の初期値
        private static readonly int StartX = 160;
        // マスのY座標の初期値
        private static readonly int StartY = 80;
        // マスの配置間隔
        private static readonly int SqSpace = 60;
        // マスのサイズ
        private static readonly int SqSize = 55;
        // マス
        private readonly TextBox[,] txtSquares = new TextBox[Num, Num];
        // マスの数字
        private readonly int[] sqVals = new int[Max];

        /// <summary>
        /// コンポーネントを設定
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // マスを作成
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

                    // 3×3マスごとに背景色を設定
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
            // タイムアウト用オブジェクト
            using var cts = new CancellationTokenSource();
            try
            {
                // 解読完了フラグ
                var flg = false;
                // タイムアウトを設定
                cts.CancelAfter(5000);
                // 解読開始
                SetSquareValue();
                SquareSearch(0, ref flg, cts.Token);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("タイムアウトしました。", "タイムアウト", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// マスを初期化
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            foreach (var t in txtSquares) t.Text = "";
        }

        /// <summary>
        /// マスの数字を設定
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
        /// マスの探索
        /// </summary>
        /// <param name="x">マスの添字</param>
        /// <param name="flg">解読完了フラグ</param>
        /// <param name="ct">タイムアウト用オブジェクト</param>
        private void SquareSearch(int x, ref bool flg, CancellationToken ct)
        {
            // タイムアウトの場合、例外処理を投げる
            ct.ThrowIfCancellationRequested();

            if (x > Max - 1)
            {
                // 解読結果を出力
                PrintSquare();
                flg = true;
            }
            else
            {
                // マスが空白でない場合、次の探索へ移行
                if (sqVals[x] != 0) SquareSearch(x + 1, ref flg, ct);
                else
                {
                    // 1~9の数字を順にマスに入れる
                    for (var n = 1; n <= Num; n++)
                    {
                        if (flg) break;
                        if (Check(n, x))
                        {
                            sqVals[x] = n;
                            SquareSearch(x + 1, ref flg, ct); // 次の探索
                            sqVals[x] = 0; // マスの初期化
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解読結果をマスに出力
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
        /// 数字の重複チェック
        /// </summary>
        /// <param name="n">チェック対象の数字</param>
        /// <param name="x">チェック対象のマスの添字</param>
        /// <returns>真偽値</returns>
        private bool Check(int n, int x)
        {
            // 横1行の左端のマスの添字
            var rowTop = x / Num * Num;
            // 縦1列の上端のマスの添字
            var columnTop = x % Num;
            // 3×3マスの左上端のマスの添字
            var frameTop = x - x / Num * Num % (Num * Frame) - x % Frame;

            // 横1行・縦1列の重複チェック
            for (var i = 0; i < Num; i++)
            {
                if (sqVals[rowTop + i] == n) return false;
                if (sqVals[columnTop + i * Num] == n) return false;
            }
            // 3×3マスの重複チェック
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