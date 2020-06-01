function Generate-Password ($IsProductionEnvironment, $length = 20, $nonAlphaChars = 5) {
    if ($IsProductionEnvironment -eq 'false') {
        $password = '@ttJlDg4j(k#_gAzzWU7';
    }
    else {
        Add-Type -AssemblyName System.Web
		
        [char[]] $illegalChars = @(':', '/', '\', '@', '''', '"', ';', '.', '+', '#')

        do {
            $hasIllegalChars = $false
            $password = [System.Web.Security.Membership]::GeneratePassword($length, $nonAlphaChars)

            $illegalChars | ForEach-Object {
                if ($password -like "*$_*") {
                    $hasIllegalChars = $true
                }
            }
        } while ($hasIllegalChars)
    }

    ConvertTo-SecureString $password -AsPlainText -Force
}