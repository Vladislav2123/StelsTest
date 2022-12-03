public class DefaultUIBar : UIBar
{
    protected override void RefreshImage()
    {
        FillImage.fillAmount = FillAmount;
    }
}
