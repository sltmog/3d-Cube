double Alpha = 0, Beta = 0, Gamma = 0;
double x, y, z;
double xp, yp;
double idx;
double cubeWidth;
int width = 160, height = 44;
double distanceFromCam = 100;
double ooz;
double horizontalOffset;
double K1 = 40;
double[] zBuffer = new double[160 * 44];
char[] buffer = new char[160 * 44];
double incrementSpeed = 0.5;

// Calcul de la position X après les différentes roations
double calculateX(double i, double j, double k)
{
    return j * Math.Sin(Alpha) * Math.Sin(Beta) * Math.Cos(Gamma) - k * Math.Cos(Alpha) * Math.Sin(Beta) * Math.Cos(Gamma) +
        j * Math.Cos(Alpha) * Math.Sin(Gamma) + k * Math.Sin(Alpha) * Math.Sin(Gamma) + i * Math.Cos(Beta) * Math.Cos(Gamma);
}

// Calcul de la position Y après les différentes rotations
double calculateY(double i, double j, double k)
{
    return j * Math.Cos(Alpha) * Math.Cos(Gamma) + k * Math.Sin(Alpha) * Math.Cos(Gamma) -
           j * Math.Sin(Alpha) * Math.Sin(Beta) * Math.Sin(Gamma) + k * Math.Cos(Alpha) * Math.Sin(Beta) * Math.Sin(Gamma) -
           i * Math.Cos(Beta) * Math.Sin(Gamma);
}

// Calcul de la position Z après les différentes rotations
double calculateZ(double i, double j, double k)
{
    return k * Math.Cos(Alpha) * Math.Cos(Beta) - j * Math.Sin(Alpha) * Math.Cos(Beta) + i * Math.Sin(Beta);
}

void CalculPostitionSurface(double cubeX, double cubeY, double cubeZ, char ch)
{
    x = calculateX(cubeX, cubeY, cubeZ);
    y = calculateY(cubeX, cubeY, cubeZ);
    z = calculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

    ooz = 1 / z;

    xp = (int)(width / 2 + horizontalOffset + K1 * ooz * x * 2);
    yp = (int)(height / 2 + K1 * ooz * y);

    idx = xp + yp * width;
    if (idx >= 0 && idx < width * height)
    {
        if (ooz > zBuffer[Convert.ToInt32(idx)])
        {
            zBuffer[Convert.ToInt32(idx)] = ooz;
            buffer[Convert.ToInt32(idx)] = ch;
        }
    }
}

while (true)
{
    // Console.Clear();
    for (int i = 0; i < buffer.Length; i++)
    {
        buffer[i] = ' ';
        zBuffer[i] = 0;
    }
    cubeWidth = 20;
    horizontalOffset = 2 * cubeWidth;
    // first cube
    for (double cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed)
    {
        for (double cubeY = -cubeWidth; cubeY < cubeWidth;
             cubeY += incrementSpeed)
        {
            CalculPostitionSurface(cubeX, cubeY, -cubeWidth, '@');
            CalculPostitionSurface(cubeWidth, cubeY, cubeX, '$');
            CalculPostitionSurface(-cubeWidth, cubeY, -cubeX, '~');
            CalculPostitionSurface(-cubeX, cubeY, cubeWidth, '#');
            CalculPostitionSurface(cubeX, -cubeWidth, -cubeY, ';');
            CalculPostitionSurface(cubeX, cubeWidth, cubeY, '+');
        }
    }
    Console.Clear();
    for (int k = 0; k < width * height; k++)
    {
        if (k % width == 0) Console.Write("\n");
        else Console.Write(buffer[k]);
    }

    Alpha += 0.05;
    Beta += 0.05;
    Gamma += 0.01;
}
