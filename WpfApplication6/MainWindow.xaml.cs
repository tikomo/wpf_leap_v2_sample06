/*
 * http://www.buildinsider.net/small/leapmotionfirstimp/01
 * 
 * 
 * 
 * v2から？ empty -> isempty に変更になってる
 * 
 * aaa
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Leap;
using System.Diagnostics; // Controller

namespace WpfApplication6
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller leapController;
        LeapListener leapListener;
        int cnt = 0;

        public MainWindow()
        {
            InitializeComponent();

            // サンプルのリスナーとコントローラーを作成
            leapListener = new LeapListener();
            leapController = new Controller();
            
            // サンプルのリスナーが、コントローラーからイベントを受け取るよう設定
            leapController.AddListener(leapListener);

            leapListener.OnFrameEvent += listener_FrameReady;
            leapListener.OnConnectEvent += listener_onConnect;
            leapListener.OnDisconnectEvent += listener_onDisconnect;
            leapListener.OnInitEvent += listener_onInit;
            leapListener.OnExitEvent += listener_onExit;

        }

        void listener_onInit(Controller leap)
        {
            // ここではWPFのUIオブジェクトも触ることができる

            Debug.WriteLine("onInit");
        }
        
        void listener_onExit(Controller leap)
        {
            // ここではWPFのUIオブジェクトも触ることができる

            Debug.WriteLine("onExit");
        }

        void listener_onConnect(Controller leap)
        {
            // ここではWPFのUIオブジェクトも触ることができる

            Debug.WriteLine("onConnect");

            //
            // サークル（TYPECIRCLE）： 指で円を描く動作
            // キー・タップ（TYPEKEYTAP）： 指で（あたかもキーを押しているように）「下方向」にタップする動作
            // スクリーン・タップ（TYPESCREENTAP）： 指で（あたかもスクリーンを押しているように）「前方向」にタップする動作
            // スワイプ（TYPESWIPE）： 指を伸ばした状態の手で直線を描く動作
            //
            leapController.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            leapController.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            leapController.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            leapController.EnableGesture(Gesture.GestureType.TYPESWIPE);
        }
        
        void listener_onDisconnect(Controller leap)
        {
            // ここではWPFのUIオブジェクトも触ることができる

            Debug.WriteLine("onDisconnect");
        }
        
        void listener_FrameReady(Controller leap)
        {
            // ここではWPFのUIオブジェクトも触ることができる
            // cnt++;
            // Debug.WriteLine("Hello World " + cnt.ToString("000"));

            //
            // フレーム情報を取得する
            //
            
            //
            // Handクラス： 検出された手の物理的な特徴をレポート
            // HandListクラス： Handオブジェクトのリスト。FrameオブジェクトのHandsプロパティで取得できる
            // Fingerクラス： トラッキングしている指を表現
            // FingerListクラス： Fingerオブジェクトのリスト。FrameオブジェクトのFingersプロパティで取得できる
            // Toolクラス： トラッキングしている道具（例えばペンなど）を表現
            // ToolListクラス： Toolオブジェクトのリスト。FrameオブジェクトのToolsプロパティで取得できる
            // Gestureクラス： 認識されたユーザーの動きを表現
            // GestureListクラス： Gestureオブジェクトのリスト。FrameオブジェクトのGesturesメソッドで取得できる
            // Vector構造体： 3次元ベクトル情報を表現
            //

            // 最新のフレームを取得して、基本情報をレポートする
            Leap.Frame frame = leapController.Frame();

            /*
            Debug.WriteLine("フレームID: " + frame.Id
                  + ", タイムスタンプ: " + frame.Timestamp
                  + ", 手の数: " + frame.Hands.Count
                  + ", 指の数: " + frame.Fingers.Count
                  + ", 道具の数: " + frame.Tools.Count
                  + ", ジェスチャーの数: " + frame.Gestures().Count);
            */
            /*
            if (!frame.Hands.IsEmpty)
            {
                // 1つ目の手を取得
                Hand hand = frame.Hands[0];

                // 手に指があるかチェック
                FingerList fingers = hand.Fingers;
                if (!fingers.IsEmpty)
                {
                    // 手の指先の平均的な位置を計算
                    Leap.Vector avgPos = Leap.Vector.Zero;
                    foreach (Finger finger in fingers)
                    {
                        avgPos += finger.TipPosition;
                    }
                    avgPos /= fingers.Count;
                    Debug.WriteLine("手には、" + fingers.Count
                      + "本の指があり、指先の平均的な位置は: " + avgPos);
                }

                // 手の球半径と手のひらの位置を取得
                Debug.WriteLine("手の球半径: " + hand.SphereRadius.ToString("n2")
                      + " mm, 手のひらの位置: " + hand.PalmPosition);

                // 手のひらの法線ベクトルと（手のひらから指までの）方向を取得
                Leap.Vector normal = hand.PalmNormal;
                Leap.Vector direction = hand.Direction;

                // 手のピッチとロールとヨー角を計算
                Debug.WriteLine("手のピッチ: " + direction.Pitch * 180.0f / (float)Math.PI + " 度, "
                      + "ロール: " + normal.Roll * 180.0f / (float)Math.PI + " 度, "
                      + "ヨー角: " + direction.Yaw * 180.0f / (float)Math.PI + " 度");
            }
            */
            //
            // ジェスチャの取得
            //
            // 全ジェスチャーを取得して、個別に処理する
            GestureList gestures = frame.Gestures();
            for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];

                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPECIRCLE:
                        CircleGesture circle = new CircleGesture(gesture);

                        // 回転方向を計算
                        String clockwiseness;
                        if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 4)
                        {
                            // 角度が90度以下なら、時計回り
                            clockwiseness = "時計回り";
                        }
                        else
                        {
                            clockwiseness = "反時計回り";
                        }

                        float sweptAngle = 0;

                        // 最後のフレームから回った角度を計算
                        if (circle.State != Gesture.GestureState.STATESTART)
                        {
                            CircleGesture previousUpdate = new CircleGesture(leapController.Frame(1).Gesture(circle.Id));
                            sweptAngle = (circle.Progress - previousUpdate.Progress) * 360;
                        }

                        label1.Content = "サークル";

                        Debug.WriteLine("サークルID: " + circle.Id
                                 + ", " + circle.State
                                 + ", 進度: " + circle.Progress
                                 + ", 半径: " + circle.Radius
                                 + ", 角度: " + sweptAngle
                                 + ", " + clockwiseness);
                        break;

                    case Gesture.GestureType.TYPESWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        Debug.WriteLine("スワイプID: " + swipe.Id
                                 + ", " + swipe.State
                                 + ", 位置: " + swipe.Position
                                 + ", 方向: " + swipe.Direction
                                 + ", スピード: " + swipe.Speed);

                        label1.Content = "スワイプ";

                        break;

                    case Gesture.GestureType.TYPEKEYTAP:
                        KeyTapGesture keytap = new KeyTapGesture(gesture);
                        Debug.WriteLine("キー・タップID: " + keytap.Id
                                 + ", " + keytap.State
                                 + ", 位置: " + keytap.Position
                                 + ", 方向: " + keytap.Direction);

                        label1.Content = "キー・タップ";

                        break;

                    case Gesture.GestureType.TYPESCREENTAP:
                        ScreenTapGesture screentap = new ScreenTapGesture(gesture);
                        Debug.WriteLine("スクリーン・タップID: " + screentap.Id
                                 + ", " + screentap.State
                                 + ", 位置: " + screentap.Position
                                 + ", 方向: " + screentap.Direction);

                        label1.Content = "スクリーン・タップ";

                        break;

                    default:
                        Debug.WriteLine("未知のジェスチャー・タイプです。");

                        label1.Content = "未知のジェスチャー・タイプです。";

                        break;
                }
            }

            if (!frame.Hands.IsEmpty || !frame.Gestures().IsEmpty)
            {
                Debug.WriteLine("");
            }
        }
    }
}
