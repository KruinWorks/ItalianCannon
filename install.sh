echo '                                            '
echo '____________       __________               '
echo '____  _/_  /______ ___  /__(_)_____ _______ '
echo ' __  / _  __/  __ `/_  /__  /_  __ `/_  __ \'
echo '__/ /  / /_ / /_/ /_  / _  / / /_/ /_  / / /'
echo '/___/  \__/ \__,_/ /_/  /_/  \__,_/ /_/ /_/ '
echo '                                            '
echo '_________                                   '
echo '__  ____/_____ ___________________________  '
echo '_  /    _  __ `/_  __ \_  __ \  __ \_  __ \ '
echo '/ /___  / /_/ /_  / / /  / / / /_/ /  / / / '
echo '\____/  \__,_/ /_/ /_//_/ /_/\____//_/ /_/  '
echo '                                            '
# Initiate variables.
GIT_REPO='https://github.com/KruinWorks/ItalianCannon.git'
PLATFORM=`uname -r`
APP_DIR='/ItalianCannon'
# Done with directories.
echo '==> Configuring working directories...'
mkdir 'ItalianCannon'
cd 'ItalianCannon'
# Detect DOTNET Runtime.
WHICH_OUTPUT=`which dotnet`
if [[ $WHICH_OUTPUT = *"no dotnet"* ]]; then
  echo "Runtime not found. Please install the dotnet runtime before executing this script."
  exit 1
fi
WGET_OUTPUT=`which wget`
if [[ $WGET_OUTPUT = *"no wget"* ]]; then
  echo "Wget required."
  exit 1
fi
# Download files.
echo "==> Installing ItalianCannon at: $PWD$APP_DIR"
echo '==> Downloading pre-compiled executables...'
wget 'https://ci.appveyor.com/api/projects/Elepover/ItalianCannon/artifacts/ItalianCannon.zip'
echo '==> Extracting...'
unzip ItalianCannon.zip
echo '==> Generating configurations file...'
dotnet ItalianCannon.dll
echo '==> Installation complete.'
exit 0