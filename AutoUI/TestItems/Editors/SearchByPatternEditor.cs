using System.Linq;
using System.Windows.Forms;

namespace AutoUI.TestItems.Editors
{
    public partial class SearchByPatternEditor : UserControl, ITestItemEditor
    {
        public SearchByPatternEditor()
        {
            InitializeComponent();
        }

        public SearchByPatternImage TestItem;
        public void Init(AutoTestItem item)
        {
            TestItem = item as SearchByPatternImage;
            if (TestItem.Pattern != null && TestItem.Pattern.Items.Any())
            {
                pictureBox1.Image = TestItem.Pattern.Items.First().Bitmap;
            }
        }
    }
}
