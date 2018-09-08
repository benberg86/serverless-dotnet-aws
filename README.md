# serverless-dotnet-aws
Setup and configuration for developing and deploying serverless framework .net core 2.1 c# applications on mac osx
- Install vs code
  - `brew cask install visual-studio-code`
- Install c# extension from within vs code
- Install dotnet core sdk
  - ```sudo mkdir -p /opt/dotnet
# staff group is specific to macOS - Linux will be different
sudo chown -R $(whoami):staff /opt/dotnet
curl -o dotnet-install.sh https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
# Install from the 2.1 channel
./dotnet-install.sh -c 2.1 -i /opt/dotnet
# Symlink the binary
ln -s /opt/dotnet/dotnet /usr/local/bin
```
- Install serverless framework
  - Install nodejs
    - ```
brew install nodejs
npm install -g serverless
```
create project
```
sls create --template aws-csharp --name projectname
```