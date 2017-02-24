using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leap;
using System.Runtime.InteropServices;

namespace LeapTest
{
    public partial class Form1 : Form, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;

        public Form1()
        {
            InitializeComponent();

            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
        }

        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        
                        break;
                    case "onConnect":
                        connectHandler();
                        break;
                    case "onFrame":
                        
                        detectHandPosition(this.controller.Frame());
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }

        public void connectHandler()
        {
            //enable all the gestures
            this.controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
        }

        /// <summary>
        /// hier starten we met de handdetectie
        /// ook letters worden hier herkent
        /// Neel Borghs
        /// </summary>
        
        //Teller voor letters uit te lezen
        int i = 0;
        int intTimer = 6;
        //Boolean dat bekijkt of hand is uitgelezen
        int intUitgelezen = 0;
        //Variabele om ILA om te zetten naar MILA
        int intMila = 0;
        string test = null;

        float oduim = 0;
        float owijsvinger = 0;
        float omiddelvinger = 0;
        float oringvinger = 0;
        float opink = 0;

        public void detectHandPosition(Leap.Frame frame)
        {
            HandList allHands = frame.Hands;
            foreach (Hand hand in allHands)
            {
                if (intUitgelezen == 1)
                {


                    for (int i = 0; i < hand.Fingers.Count; i++)
                    {
                        Finger finger = hand.Fingers[i];

                        string fingerName = finger.ToString();
                        Vector mcpPos = finger.JointPosition(Finger.FingerJoint.JOINT_MCP);
                        Vector tipPos = finger.JointPosition(Finger.FingerJoint.JOINT_TIP);
                        float angle = (float)(mcpPos.AngleTo(tipPos) * (180 / Math.PI));
                        Vector fingerPosition = finger.TipPosition;
                        float distance = hand.PalmPosition.DistanceTo(tipPos);


                        if (i == 0)
                        {
                            txtDuim.Text = distance.ToString();
                            oduim = distance;
                            txtDuim.Text = oduim.ToString();
                        }
                        if (i == 1)
                        {
                            txtWijsvinger.Text = distance.ToString();
                            owijsvinger = distance;
                            txtWijsvinger.Text = owijsvinger.ToString();
                        }
                        if (i == 2)
                        {
                            txtMiddelvinger.Text = distance.ToString();
                            omiddelvinger = distance;
                            txtMiddelvinger.Text = omiddelvinger.ToString();
                        }
                        if (i == 3)
                        {
                            txtRingvinger.Text = distance.ToString();
                            oringvinger = distance;
                            txtRingvinger.Text = oringvinger.ToString();
                        }
                        if (i == 4)
                        {
                            txtPink.Text = distance.ToString();
                            opink = distance;
                            txtPink.Text = opink.ToString();
                        }
                    }
                    intUitgelezen = 2;

                }
                if (intUitgelezen == 2)
                {
                    float duim = 0;
                    float wijsvinger = 0;
                    float middelvinger = 0;
                    float ringvinger = 0;
                    float pink = 0;

                    float pitch = hand.Direction.Pitch;
                    float yaw = hand.Direction.Yaw;
                    float roll = hand.PalmNormal.Roll;

                    double degPitch = pitch * (180 / Math.PI); //convert to degree
                    double degYaw = yaw * (180 / Math.PI);
                    double degRoll = roll * (180 / Math.PI);

                    int intPitch = (int)degPitch;
                    int intYaw = (int)degYaw;
                    int intRoll = (int)degRoll;

                    
                    lblTekst.Text = " ";

                    for (int i = 0; i < hand.Fingers.Count; i++)
                    {
                        Finger finger = hand.Fingers[i];

                        string fingerName = finger.ToString();
                        Vector mcpPos = finger.JointPosition(Finger.FingerJoint.JOINT_MCP);
                        Vector tipPos = finger.JointPosition(Finger.FingerJoint.JOINT_TIP);
                        float angle = (float)(mcpPos.AngleTo(tipPos) * (180 / Math.PI));
                        Vector fingerPosition = finger.TipPosition;
                        float distance = hand.PalmPosition.DistanceTo(tipPos);


                        if (i == 0)
                        {
                            
                            duim = distance;
                        }
                        if (i == 1)
                        {
                            
                            wijsvinger = distance;
                        }
                        if (i == 2)
                        {
                            
                            middelvinger = distance;
                        }
                        if (i == 3)
                        {
                            
                            ringvinger = distance;
                        }
                        if (i == 4)
                        {
                            
                            pink = distance;
                        }
                    }

                    //A//
                    if ((intRoll > -30) && (oduim - duim) < 10 && (owijsvinger - wijsvinger) > 15 && (omiddelvinger - middelvinger) > 15 && (oringvinger - ringvinger) > 15 && (opink - pink) > 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "A";
                        pctGebaren.Image = Properties.Resources.a;
                    }
                    //B//
                    else if ((intRoll > -30) && (oduim - duim) > 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) < 15 && (oringvinger - ringvinger) < 15 && (opink - pink) < 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "B";
                        pctGebaren.Image = Properties.Resources.b;
                    }
                    //D//
                    else if ((intRoll > -30) && (oduim - duim) > 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) > 15 && (oringvinger - ringvinger) > 15 && (opink - pink) > 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "D";
                        pctGebaren.Image = Properties.Resources.d;
                    }
                    //E//
                    else if ((intRoll > -30) && (oduim - duim) > 10 && (owijsvinger - wijsvinger) > 15 && (omiddelvinger - middelvinger) > 15 && (oringvinger - ringvinger) > 15 && (opink - pink) > 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "E";
                        pctGebaren.Image = Properties.Resources.e;
                    }
                    //F//
                    else if ((intRoll > -30) && (oduim - duim) > 10 && (owijsvinger - wijsvinger) > 15 && (omiddelvinger - middelvinger) < 15 && (oringvinger - ringvinger) < 15 && (opink - pink) < 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "F";
                        pctGebaren.Image = Properties.Resources.f;
                    }


                    //H//
                    else if ((intRoll < -60) && (oduim - duim) < 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) < 15 && (oringvinger - ringvinger) > 15 && (opink - pink) > 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "H";
                        pctGebaren.Image = Properties.Resources.h;
                    }


                    //I//
                    else if ((intRoll > -30) && (oduim - duim) > 10 && (owijsvinger - wijsvinger) > 15 && (omiddelvinger - middelvinger) > 15 && (oringvinger - ringvinger) > 15 && (opink - pink) < 15)
                    {

                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = ("I");
                        pctGebaren.Image = Properties.Resources.i;
                    }
                    //K//
                    else if ((intRoll > -30) && (oduim - duim) > 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) < 15 && (oringvinger - ringvinger) > 15 && (opink - pink) > 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "K";
                        pctGebaren.Image = Properties.Resources.k;
                    }
                    //L//
                    else if ((intRoll > -30) && (oduim - duim) < 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) > 15 && (oringvinger - ringvinger) > 15 && (opink - pink) > 15)
                    {
                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "L";
                        pctGebaren.Image = Properties.Resources.l;
                    }
                    //W//
                    else if ((intRoll > -30) && (oduim - duim) > 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) < 15 && (oringvinger - ringvinger) < 15 && (opink - pink) > 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "W";
                        pctGebaren.Image = Properties.Resources.w;
                    }
                    //Y//
                    else if ((intRoll > -30) && (oduim - duim) < 10 && (owijsvinger - wijsvinger) > 15 && (omiddelvinger - middelvinger) > 15 && (oringvinger - ringvinger) > 15 && (opink - pink) < 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 150);
                        lblTekst.Text = "Y";
                        pctGebaren.Image = Properties.Resources.y;
                    }
                    //Delete//
                    else if ((intRoll > -30) && (oduim - duim) < 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) > 15 && (oringvinger - ringvinger) > 15 && (opink - pink) < 15)
                    {

                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 80);
                        lblTekst.Text = "<";
                        pctGebaren.Image = Properties.Resources.delete;
                    }
                    //SPATIE//
                    else if ((intRoll > -30) && (oduim - duim) < 10 && (owijsvinger - wijsvinger) < 15 && (omiddelvinger - middelvinger) < 15 && (oringvinger - ringvinger) < 15 && (opink - pink) < 15)
                    {


                        lblTekst.Font = new Font(lblTekst.Font.FontFamily, 80);
                        lblTekst.Text = " ";
                        pctGebaren.Image = null;
                        pctGebaren.Image = Properties.Resources.spatie;
                    }



                    txtRoll.Text = intRoll.ToString();



                    /// <summary>
                    /// Kijken of vorige 100 letters dezelfde zijn.
                    /// Zoja ? ==> letter mag niet nog eens voorkomen
                    /// ZoNee ? ==> Nieuwe letter mag getoond worden
                    /// </summary>

                    txtTekst.Text += lblTekst.Text;
                    if (i == 0)
                    {
                        test = new String((char.Parse(" ")), 99);
                    }
                    {
                        test = new String((char.Parse(lblTekst.Text)), 99);
                    }


                    if (i < 105) //40
                    {
                        txtResultaat.Text = lblTekst.Text;
                    }
                    else
                    {
                        if (txtTekst.Text.Substring(i, 1) != txtTekst.Text.Substring(i - 100, 1) && txtTekst.Text.Substring(i - 99, 99) == test && txtTekst.Text.Substring(i, 1) != txtTekst.Text.Substring(i - 100, 1))
                        {


                            txtResultaat.Text += txtTekst.Text.Substring(i, 1);
                            pctGebaren.BackColor = Color.MediumSeaGreen;
                            if (lblTekst.Text == "<")
                            {
                                txtResultaat.Text = "";
                            }

                        }
                        else
                        {

                        }
                        if (txtTekst.Text.Substring(i - 1, 1) != lblTekst.Text)
                        {
                            pctGebaren.BackColor = Color.Transparent;
                        }
                    }

                    i += 1;

                    //HALL naar HALLO//
                    if (txtResultaat.Text.Length > 5)
                    {
                        if (txtResultaat.Text.Substring((txtResultaat.Text.Length - 5), 5) == "HALL ")
                        {

                            txtResultaat.Text = txtResultaat.Text.Substring(0, txtResultaat.Text.Length - 1) + "O ";

                        }
                    }

                    //BE naar BEN//
                    if (txtResultaat.Text.Length > 3)
                    {
                        if (txtResultaat.Text.Substring((txtResultaat.Text.Length - 3), 3) == "BE ")
                        {
                            txtResultaat.Text = txtResultaat.Text.Substring(0, txtResultaat.Text.Length - 1) + "N ";
                        }

                        if (txtResultaat.Text.Substring((txtResultaat.Text.Length - 3), 3) == "EE ")
                        {
                            txtResultaat.Text = txtResultaat.Text.Substring(0, txtResultaat.Text.Length - 1) + "N ";
                        }
                    }

                    //ILA naar MILA//
                    if (txtResultaat.Text.Length > 3)
                    {
                        if (txtResultaat.Text.Substring((txtResultaat.Text.Length - 3), 3) == "ILA" && intMila < 1)
                        {
                            intMila = 1;
                            txtResultaat.Text = txtResultaat.Text.Substring(0, txtResultaat.Text.Length - 3) + "MILA";


                        }
                    }
                }




            }
        }

        //Geen blinkende cursor tonen in actieve textbox
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        private void txtResultaat_TextChanged(object sender, EventArgs e)
        {
            HideCaret(txtResultaat.Handle);
        }

        private void tmrUitlezen_Tick(object sender, EventArgs e)
        {
            if (intTimer == 0)
            {
                intUitgelezen = 1;
                lblTimer.Visible = false;
                lblInfoTimer.Visible = false;
                pctHand.Visible = false;
                tmrUitlezen.Stop();
                detectHandPosition(controller.Frame());
                txtResultaat.Visible = true;
                lblInfoWaarde.Visible = true;
                
            }
            else
            {
                intTimer -= 1;
                lblTimer.Text = intTimer.ToString();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrUitlezen.Start();
        }
    }

    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }

    public class LeapEventListener : Listener
    {
        ILeapEventDelegate eventDelegate;

        public LeapEventListener(ILeapEventDelegate delegateObject)
        {
            this.eventDelegate = delegateObject;
        }
        public override void OnInit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onInit");
        }
        public override void OnConnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onConnect");
        }
        public override void OnFrame(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onFrame");
        }
        public override void OnExit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onExit");
        }
        public override void OnDisconnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDisconnect");
        }
    }
}
