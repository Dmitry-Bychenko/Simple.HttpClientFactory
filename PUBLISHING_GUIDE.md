# Publishing Guide for Simple.HttpClientFactory

This guide walks you through publishing Simple.HttpClientFactory to NuGet.org.

## Pre-Publication Verification

### 1. Run Final Tests & Build

```powershell
# Clean build
dotnet clean

# Build in Release mode
dotnet build -c Release

# Run all tests
dotnet test -c Release
# Expected: All 7 tests pass
```

### 2. Verify Package Contents

```powershell
# Generate package
dotnet pack -c Release -o bin

# List package contents (view as tar/zip)
# Expected files:
# - lib/net10.0/*.dll
# - lib/net10.0/*.xml (documentation)
# - icon.png
# - README.md
# - LICENSE
# - [Content_Types].xml
```

### 3. Test Installation Locally

```powershell
# Create a test project
mkdir test_nuget_install
cd test_nuget_install
dotnet new console

# Add the local package
dotnet add package Simple.HttpClientFactory --version 1.0.0 --source ../Simple.HttpClientFactory/bin/Release

# Test usage
$content = @"
using Simple.HttpClientFactory;

var factory = new DefaultHttpClientFactory();
var client = factory.CreateClient("test");
Console.WriteLine("Package installed successfully!");
"@

Set-Content -Path Program.cs -Value $content

# Run
dotnet run
```

## Publishing to NuGet.org

### Step 1: Create NuGet.org Account

1. Visit https://www.nuget.org
2. Click "Register" 
3. Create account with email and password
4. Verify email address

### Step 2: Generate API Key

1. Log in to https://www.nuget.org
2. Go to Account Settings > API Keys
3. Click "Create" to generate a new API key
4. **Save this key securely** (you'll only see it once)
5. Copy the key to clipboard

### Step 3: Configure Local NuGet (Optional)

Store your API key securely:

```powershell
# On Windows (stores in %AppData%\NuGet\NuGet.Config)
dotnet nuget update source nuget.org -u "__USERNAME__" -p "YOUR_API_KEY" --store-password-in-clear-text

# Or: Always provide key with each push (safer)
# (Use the -k parameter shown in Step 4 below)
```

### Step 4: Publish Package

```powershell
# Navigate to project directory
cd C:\Works\CS\Simple.HttpClientFactory

# Build release package
dotnet pack -c Release -o bin

# Push to NuGet.org
$apiKey = "YOUR_API_KEY_HERE"
$packagePath = "bin\Release\Simple.HttpClientFactory.1.0.0.nupkg"

dotnet nuget push $packagePath `
  --source https://api.nuget.org/v3/index.json `
  --api-key $apiKey `
  --skip-duplicate

# You should see:
# Pushing Simple.HttpClientFactory.1.0.0.nupkg to 'https://api.nuget.org/v3/index.json'...
# Your package was pushed.
```

### Step 5: Monitor Publishing

1. Wait 5-10 minutes for processing
2. Visit: https://www.nuget.org/packages/Simple.HttpClientFactory
3. Verify:
   - Package appears in search
   - Version 1.0.0 is displayed
   - Icon renders correctly
   - Description shows properly
   - Tags are visible
   - License badge shows MIT

### Step 6: Publish Symbol Package (optional)

The `.snupkg` file enables debugging with NuGet symbol servers:

```powershell
# The .snupkg should be auto-generated
$symbolPackage = "bin\Release\Simple.HttpClientFactory.1.0.0.snupkg"

dotnet nuget push $symbolPackage `
  --source https://api.nuget.org/v3/index.json `
  --api-key $apiKey
```

## Post-Publication

### Create GitHub Release

```powershell
# Create and push Git tag
git tag v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0
```

Then on GitHub:
1. Go to Releases
2. Click "Draft a new release"
3. Select tag "v1.0.0"
4. Title: "Simple.HttpClientFactory 1.0.0"
5. Description: Copy content from CHANGELOG.md
6. Publish release

### Announce Package

Share on:
- Twitter/X: Share announcement with link
- Reddit: r/dotnet community
- Dev.to: Write blog post
- LinkedIn: Professional network
- Discord/Slack communities: Relevant .NET channels

### Monitor & Support

1. Monitor package downloads on NuGet.org
2. Watch GitHub issues for feedback
3. Respond to issues promptly
4. Consider creating Release Notes blog post

## Troubleshooting

### Issue: "Invalid API key"
**Solution**: Verify the API key is correct and not expired. Generate a new one from NuGet.org.

### Issue: "Package already exists"
**Solution**: Use `--skip-duplicate` flag or bump version number for a new release.

### Issue: "Icon not found"
**Solution**: Ensure `icon.png` is in the project root and properly configured in .csproj with `Pack="true"`.

### Issue: Package takes long time to appear
**Solution**: NuGet.org can take 10-30 minutes. Check back later.

### Issue: "NU1902: Warning as Error"
**Solution**: Fix security vulnerabilities in dependencies before publishing.

## Version Bumping for Future Releases

For v1.0.1 (patch) or v1.1.0 (minor):

```powershell
# 1. Update version in .csproj
# <Version>1.0.1</Version>

# 2. Update CHANGELOG.md with new version and changes

# 3. Build and test
dotnet clean
dotnet build -c Release
dotnet test

# 4. Commit and tag
git add .
git commit -m "Version bump to 1.0.1"
git tag v1.0.1

# 5. Pack and publish
dotnet pack -c Release -o bin
dotnet nuget push "bin\Release\Simple.HttpClientFactory.1.0.1.nupkg" -s https://api.nuget.org/v3/index.json -k YOUR_API_KEY
```

## Semantic Versioning Guide

- **1.0.0** - Initial stable release
- **1.0.1** - Bug fixes only (patch)
- **1.1.0** - New features, back-compatible (minor)
- **2.0.0** - Breaking changes (major)

## Package Maintenance

### Continuous Integration (Recommended for Future)

Consider setting up CI/CD:
- GitHub Actions to run tests on every push
- Auto-publish on tag creation
- Automated testing before release

### Deprecated Versions

If you need to deprecate a version:
1. On NuGet.org package page
2. Click "Deprecate" (admin only)
3. Select reason
4. Optionally suggest alternative version

## Security Best Practices

- ✅ Never commit API keys to GitHub
- ✅ Use GitHub Secrets for CI/CD
- ✅ Keep dependencies updated
- ✅ Monitor NuGet.org security advisories
- ✅ Rotate API keys periodically
- ✅ Use separate keys for different purposes (publish vs read)

## Helpful Links

- NuGet.org: https://www.nuget.org
- NuGet Documentation: https://docs.microsoft.com/en-us/nuget/
- Semantic Versioning: https://semver.org/
- GitHub Releases: https://docs.github.com/en/repositories/releasing-projects-on-github/about-releases
- .NET Package Distribution: https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/publish-nuget-package

---

**Package**: Simple.HttpClientFactory  
**Initial Version**: 1.0.0  
**Repository**: https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory  
**License**: MIT
