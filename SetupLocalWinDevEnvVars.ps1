# --- Setup log path
#
# - $USERPROFILE\Logs
# - Create Logs dir if not exists.
# 

$logDir = [System.Environment]::GetEnvironmentVariable("USERPROFILE") + "\Documents\Logs"
New-Item -ItemType Directory -Force -Path $logDir

$logPath = $logDir + "\poc-rest-suave.log"

echo "Setup Env. Variable: LOCALDEV_POCRESTSUAVE_LOGPATH=$logPath"
[Environment]::SetEnvironmentVariable("LOCALDEV_POCRESTSUAVE_LOGPATH", "$logPath", "User")