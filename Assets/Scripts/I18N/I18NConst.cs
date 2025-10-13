namespace I18N
{
    public class I18NConst
    {
        public static string ConsignorPerfect
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "这就是我想要的！！！" : "This is what I want!!!";
            }
        }

        public static string ConsignorGood
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "还不错！！" : "Not bad!!";
            }
        }
        public static string ConsignorNormal
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "我也有这方面的需求" : "I also have such a need.";
            }
        }
        public static string ConsignorBad
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "不太对。。" : "I don't want this..";
            }
        }
        
        public static string Settlement
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "结算" : "Settlement";
            }
        }
        public static string NextTurn
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "下一局" : "Next Turn";
            }
        }

        public static string Refuse
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "抱歉" : "Sorry";
            }
        }

        public static string Accept
        {
            get
            {
                return GameManager.Instance.CurrentI18N == I18N.CN ? "好的" : "Accept";
            }
        }
    }

    public enum I18N
    {
        CN,
        EN
    }
}