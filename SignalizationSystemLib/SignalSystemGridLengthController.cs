using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace SignalizationSystemLib
{
    public class SignalSystemGridLengthController
    {
        #region Events

        public event Action<double> OnGridLengthChanged;

        #endregion

        #region Fields
        
        double max_Length;

        double min_Lenght;

        double m_speed;

        int m_sleep;
       
        double m_GridLength;
                
        #endregion

        #region Ctor

        public SignalSystemGridLengthController(double max_Length, double speed
            , double min_Lenght = 0, int sleep = 2
            )
        {
            m_GridLength = min_Lenght;
           
            this.max_Length = max_Length;

            this.min_Lenght = min_Lenght;

            this.m_speed = speed;

            m_sleep = sleep;                        
        }

        #endregion

        #region Methods

        public void Signal()
        {
            var t = new Task(() =>
            {
                Move(true);

                Thread.Sleep(TimeSpan.FromSeconds(m_sleep));

                Move(false);
            });

            t.Start();
        }

        public void SetMaxLenght(double l)
        { 
            max_Length = l;
        }

        public void SetMinLenght(double l)
        {
            min_Lenght = l;
        }

        public void SetSpeed(double v)
        {
            m_speed = v;
        }

        public void SetSleep(int v)
        {
            m_sleep = v;
        }

        private void Move(bool forwardBackward)
        {            
            if (forwardBackward)
            {
                while (m_GridLength < max_Length)
                {
                    m_GridLength += m_speed;

                    OnGridLengthChanged?.Invoke(m_GridLength);                    
                }
            }
            else
            {
                while (m_GridLength > min_Lenght)
                {
                    m_GridLength -= m_speed;

                    OnGridLengthChanged?.Invoke(m_GridLength);                    
                }
            }
            
        }

        #endregion

    }
}
