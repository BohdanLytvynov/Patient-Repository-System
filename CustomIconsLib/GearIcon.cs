using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ViewModelBaseLib.VM;

namespace CustomIconsLib
{
    public class GearIcon : ViewModelBaseClass
    {
        #region Fields

        PointCollection m_GearPoints;

        #endregion

        #region Properties

        public PointCollection GearPoints 
        {
            get=> m_GearPoints;
            set=> Set(ref m_GearPoints, value, nameof(GearPoints));
        }

        #endregion

        #region Ctor

        public GearIcon()
        {
            m_GearPoints = new PointCollection();

            DrawGear(1, 15, 25, 10, 10, 14, 14);
        }

        #endregion

        #region Methods
        public void DrawGear(double DotStep, double prongStep, double prongLength, double Xmultmin, double Ymultmin,
            double XmultMax, double YmultMax)
        {
            double angle = 0;
            
            double low1 = 0;

            double up1 = prongStep;

            double low2 = prongStep;

            double up2 = prongStep + prongLength;

            double step = up2;
            
            while (angle <= 360)
            {
                while (angle >= low1 && angle <=up1 && angle <= 360)
                {                    
                    angle += DotStep;

                    Point p = new Point
                        (
                            Xmultmin * Math.Cos(angle * Math.PI / 180) + 15,

                            Ymultmin * Math.Sin(angle * Math.PI / 180) + 15
                        ) ;

                    GearPoints.Add(p);
                }

                low1 += step;

                up1 += step;

                while (angle >= low2 && angle <= up2 && angle <= 360)
                {                    
                    angle += DotStep;

                    Point p = new Point
                        (
                            XmultMax * Math.Cos(angle * Math.PI / 180) + 15,

                            YmultMax * Math.Sin(angle * Math.PI / 180) + 15
                        );

                    GearPoints.Add(p);
                }

                low2 += step;

                up2 += step;

            }

            
        }
        #endregion

    }
}