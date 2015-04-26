using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КачествоТП
{
    public static class Preobraz
    {
        public static float xS(float x_real, int xMax, int xmax, int xmin)
        {
            float x;
            return x = xMax * (x_real - xmin) / (xmax - xmin);
        }

        public static float yS(float y_real, int yMax, int ymax, int ymin)
        {
            float y;
            return y = yMax * (1 - (y_real - ymin) / (ymax - ymin));
        }
    }
}
