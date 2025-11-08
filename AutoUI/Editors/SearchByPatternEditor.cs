using AutoUI.Common;
using AutoUI.Editors;
using System.Linq;
using System.Windows.Forms;

namespace AutoUI.TestItems.Editors
{
    [TestItemEditor(Target = typeof(SearchByPatternImage))]
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
