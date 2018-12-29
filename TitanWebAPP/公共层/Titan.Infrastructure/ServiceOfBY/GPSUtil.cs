using System;

namespace Titan.Infrastructure.ServiceOfBY
{
    public class GPSUtil
    {
        public static double pi = 3.1415926535897932384626;
        public static double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        public static double a = 6378245.0;
        public static double ee = 0.00669342162296594323;

        public static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        public static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0
                    * pi)) * 2.0 / 3.0;
            return ret;
        }

        public static double[] transform(double lat, double lon)
        {
            if (outOfChina(lat, lon))
            {
                return new[] { lat, lon };
            }
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            return new[] { mgLat, mgLon };
        }

        public static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        /** 
         * 84 to 火星坐标系 (GCJ-02) World Geodetic System ==> Mars Geodetic System 
         * 
         * @param lat 
         * @param lon 
         * @return 
         */
        public static double[] gps84_To_Gcj02(double lat, double lon)
        {
            if (outOfChina(lat, lon))
            {
                return new[] { lat, lon };
            }
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            return new[] { mgLat, mgLon };
        }

        /** 
         * * 火星坐标系 (GCJ-02) to 84 * * @param lon * @param lat * @return 
         * */
        public static double[] gcj02_To_Gps84(double lat, double lon)
        {
            double[] gps = transform(lat, lon);
            double lontitude = lon * 2 - gps[1];
            double latitude = lat * 2 - gps[0];
            return new[] { latitude, lontitude };
        }

        /** 
         * 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 将 GCJ-02 坐标转换成 BD-09 坐标 
         * 
         * @param lat 
         * @param lon 
         */
        public static double[] gcj02_To_Bd09(double lat, double lon)
        {
            double x = lon, y = lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            double tempLon = z * Math.Cos(theta) + 0.0065;
            double tempLat = z * Math.Sin(theta) + 0.006;
            double[] gps = { tempLat, tempLon };
            return gps;
        }

        /** 
         * * 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 * * 将 BD-09 坐标转换成GCJ-02 坐标 * * @param 
         * bd_lat * @param bd_lon * @return 
         */
        public static double[] bd09_To_Gcj02(double lat, double lon)
        {
            double x = lon - 0.0065, y = lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            double tempLon = z * Math.Cos(theta);
            double tempLat = z * Math.Sin(theta);
            double[] gps = { tempLat, tempLon };
            return gps;
        }

        /**将gps84转为bd09 
         * @param lat 
         * @param lon 
         * @return 
         */
        public static double[] gps84_To_bd09(double lat, double lon)
        {
            double[] gcj02 = gps84_To_Gcj02(lat, lon);
            double[] bd09 = gcj02_To_Bd09(gcj02[0], gcj02[1]);
            return bd09;
        }

        public static double[] bd09_To_gps84(double lat, double lon)
        {
            double[] gcj02 = bd09_To_Gcj02(lat, lon);
            double[] gps84 = gcj02_To_Gps84(gcj02[0], gcj02[1]);
            //保留小数点后六位  
            gps84[0] = retain6(gps84[0]);
            gps84[1] = retain6(gps84[1]);
            return gps84;
        }

        /**保留小数点后六位 
         * @param num 
         * @return 
         */
        private static double retain6(double num)
        {
            return Math.Round(num, 6);
        }

    }
}