# serverless-dotnet-aws

Setup and configuration for developing and deploying serverless framework .net core 2.1 c# applications on mac osx

- Install vs code
  - `brew cask install visual-studio-code`
- Install c# extension from within vs code
- Install dotnet core sdk

```bash
sudo mkdir -p /opt/dotnet
# staff group is specific to macOS - Linux will be different
sudo chown -R $(whoami):staff /opt/dotnet
curl -o dotnet-install.sh https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
# Install from the 2.1 channel
./dotnet-install.sh -c 2.1 -i /opt/dotnet
# Symlink the binary
ln -s /opt/dotnet/dotnet /usr/local/bin
```

- Install nodejs and serverless framework

```bash
brew install nodejs
npm install -g serverless
```

- create project

```bash
sls create --template aws-csharp --name projectname
```

## Build

`sh build.sh`

## Deploy Service

`sls deploy -v`

## Deploy Function

`serverless deploy function -f hello`

## Invoke Function

`serverless invoke -f hello -l`

## Cleanup

`serverless remove`

Setup and configuration for developing and deploying serverless framework .net core 2.1 c# applications on windows
RUN AS ADMIN

- Install vs code
  - `choco install vscode -y`
- Install c# extension from within vs code
- Install dotnet core sdk
  - `choco install dotnetcore-sdk -y`
- Install nodejs
  - `choco install nodejs -y`
- Refresh
  - `refreshenv`
- Install Serverless Framework
  - `npm install -g serverless`