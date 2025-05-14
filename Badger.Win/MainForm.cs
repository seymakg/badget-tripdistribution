using FileSystem.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using Badger.Win.FileLoaders;
using Badger.Solvers.Gravity;
using Badger.Algebra.Matrix;
using Badger.Algebra.Array;
using Badger.Solvers.GameTheory;
using Microsoft.SolverFoundation.Services;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using System.IO;
using log4net;
using System.Configuration;
using Badger.Solvers.Ipopt;
using Cureos.Numerics;

namespace Badger.Win
{
    public partial class MainForm : Form
    {
        public double[,] CostMatrix { get; set; }

        public double[,] TripMatrix { get; set; }

        BackgroundWorker backgroundWorker;
        BackgroundWorker gameTheoryBackgroundWorker;
        BackgroundWorker timerWorker;
        RunWorkerCompletedEventHandler runWorkerCompletedEventHandler;
        Timer timer;
        Loading loading;

        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));

        public MainForm()
        {
            InitializeComponent();

            backgroundWorker = new BackgroundWorker();
            gameTheoryBackgroundWorker = new BackgroundWorker();
            timerWorker = new BackgroundWorker();
            timerWorker.DoWork += TimerWorker_DoWork;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Disposed += Timer_Disposed;
        }

        private void Timer_Disposed(object sender, EventArgs e)
        {
            if (loading != null)
            {
                loading.Close();
                loading.Dispose();
                loading = null;
            }
        }

        private void TimerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (loading == null)
            {
                loading = new Loading();
                loading.Show();
            }
            else
            {
                var text = loading.GetText();

                int currentValue;
                int.TryParse(text, out currentValue);
                currentValue++;
                string currentText = currentValue.ToString();

                loading.SetText(currentText);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miUploadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            fileDialog.ShowDialog();

            try
            {
                var filePath = fileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    LoadExcelFile(fileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void LoadExcelFile(string filePath)
        {
            ExcelFileLoader excelFileLoader = new ExcelFileLoader(filePath);
            excelFileLoader.FormClosing += ExcelFileLoader_FormClosing;
            excelFileLoader.Show();
        }

        private void ExcelFileLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExcelFileLoader excelFileLoader = (ExcelFileLoader)sender;

            CostMatrix = excelFileLoader.CostMatrix;
            TripMatrix = excelFileLoader.TripMatrix;

            LoadDataView(gvCost, excelFileLoader.Names, CostMatrix.ToStringMatrix());

            LoadDataView(gvTrip, excelFileLoader.Names, TripMatrix.ToStringMatrix());
        }

        private void LoadDataView(DataGridView gridView, string[] columns, string[,] dataValues)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            for (int i = 0; i < columns.Length; i++)
            {
                dataTable.Columns.Add(new DataColumn(columns[i], typeof(double)));
            }

            for (int rowIndex = 0; rowIndex < dataValues.GetLength(0); rowIndex++)
            {
                DataRow row = dataTable.NewRow();
                for (int columnIndex = 0; columnIndex < dataValues.GetLength(1); columnIndex++)
                {
                    row[columnIndex] = dataValues[rowIndex, columnIndex];
                }

                dataTable.Rows.Add(row);
                row.AcceptChanges();
            }

            gridView.AllowUserToAddRows = false;
            gridView.AllowUserToDeleteRows = false;
            gridView.AllowUserToResizeRows = false;
            gridView.ReadOnly = true;
            gridView.DataSource = dataTable;
            gridView.Dock = DockStyle.Fill;
        }

        private void miGravity_Click(object sender, EventArgs e)
        {
            if (TripMatrix == null || CostMatrix == null)
            {
                MessageBox.Show("Please load data first.");
                return;
            }

            GravitySolver gravitySolver = new GravitySolver();
            IGravitySolution solution = gravitySolver.Solve(TripMatrix, CostMatrix);

            // Save similar to game theory
            int zoneCount = CostMatrix.GetLength(0);
            int matrixLength = zoneCount * zoneCount;
            int paramLength = (zoneCount * 2) + matrixLength;
            double[] arrayA = MatrixHelper.Zeros(zoneCount);
            double[] arrayB = MatrixHelper.Zeros(zoneCount);
            double[,] matrixLike = solution.Trips;
            double[] solutionValues = new double[paramLength];

            Array.Copy(arrayA, 0, solutionValues, 0, zoneCount);
            Array.Copy(arrayB, 0, solutionValues, zoneCount, zoneCount);

            for (int i = 0; i < zoneCount; i++)
            {
                for (int j = 0; j < zoneCount; j++)
                {
                    solutionValues[(zoneCount * 2) + zoneCount * i + j] = matrixLike[i, j];
                }
            }

            GameTheorySolution gravitySolution = new GameTheorySolution(NonlinearResult.LocalOptimal, 0, solutionValues);
            SaveSolution(gravitySolution, DateTime.Now.ToString("yyyyMMddHHmmss"), 0);

            ResultViewer resultViewer = new ResultViewer();
            resultViewer.Show();

            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(solution.Warning))
            {
                stringBuilder.AppendLine(solution.Warning);
            }
            stringBuilder.AppendLine($"RMSE: {string.Format("{0:0.0000}", solution.RMSE)}");
            stringBuilder.AppendLine($"Beta: {string.Format("{0:0.0000}", solution.Beta)}");

            stringBuilder.AppendLine($"Observed TLD:");
            stringBuilder.AppendLine($"----------------------------------------------");
            stringBuilder.AppendLine($"{ArrayHelper.WriteArray(solution.ObservedTLD, true)}");

            stringBuilder.AppendLine($"Modelled TLD:");
            stringBuilder.AppendLine($"----------------------------------------------");
            stringBuilder.AppendLine($"{ArrayHelper.WriteArray(solution.ModelledTLD, true)}");

            stringBuilder.AppendLine($"Trip Solution:");
            stringBuilder.AppendLine($"----------------------------------------------");
            stringBuilder.AppendLine($"{MatrixHelper.WriteMatrix(solution.Trips)}");

            resultViewer.SetResult(stringBuilder.ToString());

            resultViewer.FormClosed += ResultViewer_FormClosed;
            timer.Stop();
        }

        private void miGameTheory_Click(object sender, EventArgs e)
        {
            previousResult = null;
            if (TripMatrix == null || CostMatrix == null)
            {
                MessageBox.Show("Please load data first.");
                return;
            }

            runWorkerCompletedEventHandler = new RunWorkerCompletedEventHandler((object worker, RunWorkerCompletedEventArgs eventArgs) =>
            {
                backgroundWorker.Dispose();
                backgroundWorker.RunWorkerCompleted -= runWorkerCompletedEventHandler;

                var text = eventArgs.Result;

                gameTheoryBackgroundWorker.DoWork += GameTheoryWorker_DoWork;
                gameTheoryBackgroundWorker.RunWorkerCompleted += GameTheoryWorker_RunWorkerCompleted;
                gameTheoryBackgroundWorker.Disposed += GameTheoryWorker_Disposed;

                gameTheoryBackgroundWorker.RunWorkerAsync(text);
                timer.Start();
            });

            RunCreateFunctionTask(runWorkerCompletedEventHandler);
        }

        private void GameTheoryWorker_Disposed(object sender, EventArgs e)
        {
            gameTheoryBackgroundWorker.DoWork -= GameTheoryWorker_DoWork;
            gameTheoryBackgroundWorker.RunWorkerCompleted -= GameTheoryWorker_RunWorkerCompleted;
            gameTheoryBackgroundWorker.Disposed -= GameTheoryWorker_Disposed;
        }

        private void GameTheoryWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultViewer resultViewer = new ResultViewer();
            resultViewer.Show();
            resultViewer.SetResult(e.Result.ToString());
            resultViewer.FormClosed += ResultViewer_FormClosed;

            timer.Stop();
            gameTheoryBackgroundWorker.Dispose();
        }

        private void ResultViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Dispose();
        }

        private void GameTheoryWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                logger.Info("Starting execution of solver.");

                var functionText = e.Argument.ToString();
                int zoneCount = CostMatrix.GetLength(0);
                int variableCount = zoneCount * zoneCount + (2 * zoneCount);

                var param_x = Expression.Parameter(typeof(double[]), "x");

                var expression = DynamicExpressionParser.ParseLambda(new[] { param_x }, null, functionText);
                Func<double[], double> compiledExpression = expression.Compile() as Func<double[], double>;

                GameTheorySolver gameTheorySolver = new GameTheorySolver(variableCount, compiledExpression);
                gameTheorySolver.OnSolvingCompleted += GameTheorySolver_OnSolvingCompleted;

                var lastResult = GetLastResult();
                if (lastResult != null)
                {
                    gameTheorySolver.Solve(lastResult.ParamaterValues);
                }
                else
                {
                    gameTheorySolver.Solve();
                }

                logger.Info("Execution completed.");
                var resultText = GetLatestResult(gameTheorySolver.LastResult);
                e.Result = resultText;
            }
            catch (Exception ex)
            {
                logger.Error("Error while solving", ex);
                stringBuilder.AppendLine(ex.Message);
                stringBuilder.AppendLine(ex.StackTrace);
                e.Result = stringBuilder.ToString();
            }

        }

        private GameTheorySolution GetLastResult()
        {

            try
            {
                string[] files = Directory.GetFiles(getSaveDirectory(), "*.json");
                if (files != null && files.Length > 0)
                {
                    var lastResultFile = files.OrderByDescending(x => x).First();
                    var resultText = File.ReadAllText(lastResultFile);

                    GameTheorySolution lastSolution = JsonConvert.DeserializeObject<GameTheorySolution>(resultText);
                    return lastSolution;
                }

                return null;
            }
            catch (Exception ex)
            {
                logger.Error("Error while reading last result", ex);
                return null;
            }
        }

        double? previousResult = null;
        private void GameTheorySolver_OnSolvingCompleted(object sender, SolvingCompletedEventArgs e)
        {
            GameTheorySolver solver = (GameTheorySolver)sender;
            if (e.Solution != null)
            {
                logger.Info("Solving completed");

                SaveSolution(e.Solution, solver.InstanceUniqueId, solver.RetryCount);

                logger.Debug("Save completed.");
                if (e.Solution.NonlinearResult == NonlinearResult.Interrupted)
                {
                    if (previousResult != null && (previousResult - e.Solution.SolutionValue) < 0.00001)
                    {
                        logger.Info("Consecutive solution comparison threshold (0.00001) is reached. Do not continue.");
                    }
                    else
                    {
                        previousResult = e.Solution.SolutionValue;
                        logger.Info("Solving restarted.");
                        solver.Solve(e.Solution.ParamaterValues);
                    }
                }
                else
                {
                    logger.Info("A solution found.");
                }
            }
            else
            {
                logger.Error("No actual solution after solving completed.");
            }
        }

        private void SaveSolution(GameTheorySolution solution, string instanceUniqueId, int retryCount)
        {
            var solutionText = JsonConvert.SerializeObject(solution);
            logger.Debug($"NonlinearResult: {Enum.GetName(typeof(NonlinearResult), solution.NonlinearResult)} - Value: {string.Format("{0:0.0000}", solution.SolutionValue)}");

            string filePath = string.Format(getSavePath(), instanceUniqueId, retryCount.ToString().PadLeft(4, '0'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, solutionText);
        }

        private string GetLatestResult(GameTheorySolution solution)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int zoneCount = CostMatrix.GetLength(0);

            string resultType = Enum.GetName(typeof(NonlinearResult), solution.NonlinearResult);
            stringBuilder.AppendLine($"Result Type: {resultType}");
            stringBuilder.AppendLine($"Found Value: {string.Format("{0:0.0000}", solution.SolutionValue)}");

            int matrixLength = zoneCount * zoneCount;
            double[] arrayA = new double[zoneCount];
            double[] arrayB = new double[zoneCount];
            double[] prematrix = new double[matrixLength];
            double[,] matrix = new double[zoneCount, zoneCount];

            Array.Copy(solution.ParamaterValues, 0, arrayA, 0, zoneCount);
            Array.Copy(solution.ParamaterValues, zoneCount, arrayB, 0, zoneCount);
            Array.Copy(solution.ParamaterValues, (2 * zoneCount), prematrix, 0, matrixLength);

            for (int i = 0; i < zoneCount; i++)
            {
                for (int j = 0; j < zoneCount; j++)
                {
                    matrix[i, j] = prematrix[zoneCount * i + j];
                }
            }

            stringBuilder.AppendLine($"Variables (A):");
            stringBuilder.AppendLine($"----------------------------------------------");
            stringBuilder.AppendLine($"{ArrayHelper.WriteArray(arrayA, true)}");

            stringBuilder.AppendLine($"Variables (B):");
            stringBuilder.AppendLine($"----------------------------------------------");
            stringBuilder.AppendLine($"{ArrayHelper.WriteArray(arrayB, true)}");

            stringBuilder.AppendLine($"Trip Solution:");
            stringBuilder.AppendLine($"----------------------------------------------");
            stringBuilder.AppendLine($"{MatrixHelper.WriteMatrix(matrix)}");

            return stringBuilder.ToString();
        }

        private void RunCreateFunctionTask(RunWorkerCompletedEventHandler onCompletedCallBack)
        {
            backgroundWorker.DoWork += CreateFunctionWorker_DoWork;
            if (onCompletedCallBack == null)
            {
                backgroundWorker.RunWorkerCompleted += CreateFunctionWorker_RunWorkerCompleted;
            }
            else
            {
                backgroundWorker.RunWorkerCompleted += onCompletedCallBack;
            }
            backgroundWorker.Disposed += CreateFunctionWorker_Disposed;
            backgroundWorker.RunWorkerAsync(onCompletedCallBack);
            timer.Start();
        }

        private void CreateFunctionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var rowSums = MatrixHelper.Sum(TripMatrix, 0);
            var columnSums = MatrixHelper.Sum(TripMatrix, 1);
            string text = Temp.ObjectiveFunctionV1.CreateFunctionText(CostMatrix.GetLength(0), CostMatrix, rowSums, columnSums);

            e.Result = text;
        }

        private void CreateFunctionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultViewer resultViewer = new ResultViewer();
            resultViewer.Show();
            resultViewer.SetResult(e.Result.ToString());
            resultViewer.FormClosed += ResultViewer_FormClosed;
            backgroundWorker.Dispose();
            timer.Stop();
        }

        private void CreateFunctionWorker_Disposed(object sender, EventArgs e)
        {
            backgroundWorker.DoWork -= CreateFunctionWorker_DoWork;
            backgroundWorker.RunWorkerCompleted -= CreateFunctionWorker_RunWorkerCompleted;
            backgroundWorker.Disposed -= CreateFunctionWorker_Disposed;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($@"
                Author: Sumeyye Seyma Kusakci Gundogar
                Email: seymakusakci@gmail.com
            ", "About");
        }

        private void saveLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentLocation = Properties.Settings.Default.FileSaveLocation;

            EditForm editForm = new EditForm();
            editForm.EditValue = currentLocation;
            editForm.EditLabel = "Save Location";

            editForm.Show();
        }

        private string getSavePath() 
        {
            string saveFolder = Properties.Settings.Default["FileSaveLocation"].ToStringOrEmpty();
            string savePath = saveFolder + @"Save_{0}_{1}.json";
            return savePath;
        }

        private string getSaveDirectory()
        {
            string saveFolder = Properties.Settings.Default["FileSaveLocation"].ToStringOrEmpty();
            return saveFolder;
        }

        private void denemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* create the IpoptProblem */
            MyIpoptProblem p = new MyIpoptProblem();

            /* allocate space for the initial point and set the values */
            double[] x = { 1.0, 5.0, 5.0, 1.0 };

            IpoptReturnCode status;

            using (var problem = new IpoptProblem(p._n, p._x_L, p._x_U, p._m, p._g_L, p._g_U, p._nele_jac, p._nele_hess,
                p.eval_f, p.eval_g, p.eval_grad_f, p.eval_jac_g, p.eval_h))
            {
                /* Set some options.  The following ones are only examples,
                   they might not be suitable for your problem. */
                //problem.AddOption("derivative_test", "second-order");
                //problem.AddOption("tol", 1e-7);
                //problem.AddOption("mu_strategy", "adaptive");
                //problem.AddOption("output_file", "hs071.txt");
                problem.SetIntermediateCallback(this.IntermediateCallback);
                /* solve the problem */
                double obj;
                status = problem.SolveProblem(x, out obj, null, null, null, null);
            }

            //Console.WriteLine("{0}{0}Optimization return status: {1}{0}{0}", Environment.NewLine, status);

            for (int i = 0; i < 4; ++i) Console.WriteLine("x[{0}]={1}", i, x[i]);

        }

        private bool IntermediateCallback(IpoptAlgorithmMode alg_mod, int iter_count, double obj_value, double inf_pr, double inf_du, double mu, double d_norm, double regularization_size, double alpha_du, double alpha_pr, int ls_trials)
        {
            return true;
        }
    }
}
