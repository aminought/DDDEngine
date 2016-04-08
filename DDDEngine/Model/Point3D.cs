﻿using MathNet.Numerics.LinearAlgebra.Double;

namespace DDDEngine.Model
{
    public class Point3D: IDrawable
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D() { }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Draw(Point3D worldPoint, Cameras.Camera camera)
        {
            var point2D = ConvertTo2D(worldPoint, camera);
            point2D.Draw();
        }

        public Point2D ConvertTo2D(Point3D worldPoint, Cameras.Camera camera)
        {
            var point2D = new Point2D();
            var worldMatrix = GetWorldMatrix(worldPoint);
            var cameraMatrix = camera.GetCameraMatrix();
            var projectionMatrix = camera.GetProjectionMatrix();
            var transformMatrix = cameraMatrix*worldMatrix;
            var pointMatrix = DenseMatrix.OfArray(new[,]
            {
                {X},
                {Y},
                {Z},
                {1}
            });
            var resultPointMatrix = transformMatrix*pointMatrix;
            var values = resultPointMatrix.Values;
            point2D.X = values[0] + Configuration.Config.Canvas.ActualWidth/2;
            point2D.Y = values[1] + Configuration.Config.Canvas.ActualHeight/2;
            return point2D;
        }

        private DenseMatrix GetWorldMatrix(Point3D point)
        {
            return DenseMatrix.OfArray(new[,]
            {
                {1, 0, 0, point.X + X},
                {0, 1, 0, point.Y + Y},
                {0, 0, 1, point.Z + Z},
                {0, 0, 0, 1}
            });
        }

    }
}