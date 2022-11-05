namespace TimerLib
{
    public class TimerSystem
    {
        #region Events

        public event Action OnTimerFinished;

        public event Action<double> OnTimerChanged;

        #endregion

        #region Fields
         
        double m_start;

        double m_tempStart;

        double m_decrement;

        public double CurrentTimeValue { get => m_start;}

        #endregion

        #region Ctor

        public TimerSystem(double start, double decrement)
        {
            this.m_start = start;

            m_tempStart = start;

            m_decrement = decrement;
        }

        #endregion

        #region Methods
        public void Reset()
        {
            m_start = m_tempStart;
        }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                while (m_start != 0)
                {
                    m_start -= m_decrement;
                }

                OnTimerFinished?.Invoke();
            });
        }

        public void Start()
        {
            while (m_start >= 0)
            {
                m_start -= m_decrement;

                OnTimerChanged?.Invoke(m_start);
            }

            OnTimerFinished?.Invoke();
        }

        public void SetNewStartTime(double start)
        {
            m_start = start;

            m_tempStart = start;
        }

        public void SetNewDecrement(double decrement)
        {
            m_decrement = decrement;
        }
        #endregion
    }
}