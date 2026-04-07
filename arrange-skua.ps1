Add-Type @"
using System;
using System.Runtime.InteropServices;

public class WinAPI {
    [DllImport("user32.dll")]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool Repaint);

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}
"@

# Constants
$SW_RESTORE = 9

# Get only Skua windows (exclude Manager)
$windows = Get-Process | Where-Object {
    $_.MainWindowTitle -match '^Skua - \d+\.\d+\.\d+\.\d+$' -and $_.MainWindowHandle -ne 0
}

$count = $windows.Count
if ($count -eq 0) {
    Write-Host "No Skua windows found."
    exit
}

# Grid calculation
$cols = [math]::Ceiling([math]::Sqrt($count))
$rows = [math]::Ceiling($count / $cols)

# Screen size
Add-Type -AssemblyName System.Windows.Forms
$screen = [System.Windows.Forms.Screen]::PrimaryScreen.WorkingArea

$width  = [int]($screen.Width / $cols)
$height = [int]($screen.Height / $rows)

# Arrange + bring to front
for ($i = 0; $i -lt $count; $i++) {
    $handle = $windows[$i].MainWindowHandle

    # Restore if minimized
    [WinAPI]::ShowWindow($handle, $SW_RESTORE) | Out-Null

    # Bring to front
    [WinAPI]::SetForegroundWindow($handle) | Out-Null

    Start-Sleep -Milliseconds 50  # helps Windows actually honor focus

    $col = $i % $cols
    $row = [math]::Floor($i / $cols)

    $x = $col * $width
    $y = $row * $height

    [WinAPI]::MoveWindow($handle, $x, $y, $width, $height, $true) | Out-Null
}

Write-Host "Arranged and focused $count Skua windows in a $cols x $rows grid."