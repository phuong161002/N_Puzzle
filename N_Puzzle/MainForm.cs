using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using N_Puzzle.Algorithms;
using System.Diagnostics;

namespace N_Puzzle
{
  public partial class MainForm : Form
  {
    public static bool IsOutOfMem;
    public static MainForm Instance;
    private PictureBox[] myPictureBoxes;
    private Image[] myImages;
    private Image blackImage;
    private Image original;
    private int[] startState;
    private int[] goalState;
    public Node startNode;
    public Node currentNode;
    public Node goalNode;
    private Task runningTask;
    private CancellationTokenSource taskStop;

    public MainForm()
    {
      InitializeComponent();
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        this.Close();
      }
      CenterToParent();
      LoadImage();
      Control.CheckForIllegalCrossThreadCalls = false;
    }

    public void LoadImage()
    {
      myPictureBoxes = new PictureBox[9];
      myPictureBoxes[0] = pictureBox1;
      myPictureBoxes[1] = pictureBox2;
      myPictureBoxes[2] = pictureBox3;
      myPictureBoxes[3] = pictureBox4;
      myPictureBoxes[4] = pictureBox5;
      myPictureBoxes[5] = pictureBox6;
      myPictureBoxes[6] = pictureBox7;
      myPictureBoxes[7] = pictureBox8;
      myPictureBoxes[8] = pictureBox9;

      myImages = new Image[10];
      myImages[0] = Properties.Resources.black;
      myImages[1] = Properties.Resources.Img1;
      myImages[2] = Properties.Resources.Img2;
      myImages[3] = Properties.Resources.Img3;
      myImages[4] = Properties.Resources.Img4;
      myImages[5] = Properties.Resources.Img5;
      myImages[6] = Properties.Resources.Img6;
      myImages[7] = Properties.Resources.Img7;
      myImages[8] = Properties.Resources.Img8;
      myImages[9] = Properties.Resources.Img9;

      blackImage = Properties.Resources.black;
      original = Properties.Resources.original;

      goalState = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
      startState = (int[])goalState.Clone();

      startNode = new Node(startState);

      currentNode = new Node(startNode.state);
      UpdateGameView(currentNode.state);
      goalNode = new Node(goalState);
    }

    private void UpdateGameView(int[] array)
    {
      for (int i = 0; i < array.Length && i < myPictureBoxes.Length; i++)
      {
        myPictureBoxes[i].Image = myImages[array[i]];
      }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      //base.OnKeyDown(e);
      switch (e.KeyCode)
      {
        case Keys.Up:
          MoveUp();
          break;
        case Keys.Down:
          MoveDown();
          break;
        case Keys.Left:
          MoveLeft();
          break;
        case Keys.Right:
          MoveRight();
          break;
      }
      Util.Print(currentNode.state);
      UpdateGameView(currentNode.state);
    }

    public void MoveUp()
    {
      if (Util.TryMove(currentNode, MoveDirection.Up, out Node nextNode))
      {
        currentNode = nextNode;
      }
    }

    public void MoveDown()
    {
      if (Util.TryMove(currentNode, MoveDirection.Down, out Node nextNode))
      {
        currentNode = nextNode;
      }
    }

    public void MoveLeft()
    {
      if (Util.TryMove(currentNode, MoveDirection.Left, out Node nextNode))
      {
        currentNode = nextNode;
      }
    }

    public void MoveRight()
    {
      if (Util.TryMove(currentNode, MoveDirection.Right, out Node nextNode))
      {
        currentNode = nextNode;
      }
    }

    private Task Solve(Node start, Node goal)
    {
      IsOutOfMem = false;
      label1.Text = "Solving...";
      return Task.Factory.StartNew(() =>
      {
        Stopwatch stopWatch = Stopwatch.StartNew();
        // BFS bfs = new BFS(start, goal);
        // Node node = bfs.Solve(start, goal);
        AStar astar = new AStar(start, goal);
        Node node = astar.Solve(start, goal);
        label1.Text = $"Elapsed Time: {stopWatch.ElapsedMilliseconds}ms";
        return node;
      }).ContinueWith(task =>
      {
        if (!IsOutOfMem)
        {
          var moves = Util.Trace(task.Result);
          ShowMove(moves);
        }
        else
        {
          label1.Text = "Out of memory! Can not solve";
          currentNode = new Node(startState);
          UpdateGameView(currentNode.state);
        }
      }).ContinueWith(task =>
      {
        currentNode = new Node(currentNode.state);
        UpdateGameView(currentNode.state);
        GC.Collect();
      });
    }

    private void ShowMove(List<Node> listNode)
    {
      listNode.ForEach(n =>
      {
        currentNode = n;
        UpdateGameView(n.state);
        Thread.Sleep(300);
      });
    }

    private void btnSolve_Click(object sender, EventArgs e)
    {
      runningTask = Solve(currentNode, goalNode);
    }

    private void btnShuffer_Click(object sender, EventArgs e)
    {
      if (int.TryParse(tbShufferItters.Text, out int num))
      {
        var newState = Util.Shuffer(currentNode.state, num);
        currentNode = new Node(newState);
        UpdateGameView(currentNode.state);
      }
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      Process proc = Process.GetCurrentProcess();
      proc.Refresh();
      var memUsaged = proc.PrivateMemorySize64 / 1024 / 1024;
      if (memUsaged > 500)
      {
        label1.Text = "Out of memory! Can not solve";
        IsOutOfMem = true;
      }
      label2.Text = $"Ram: {memUsaged} MB";
    }
  }
}
