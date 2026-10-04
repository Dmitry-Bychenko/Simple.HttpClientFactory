# Quick Reference: Publishing Simple.HttpClientFactory

## ⚡ 3-Minute Quick Start

### Before Publishing
```powershell
# 1. Verify everything works
dotnet clean
dotnet build -c Release
dotnet test
# Expected: All 7 tests PASS ✅

# 2. Generate package
dotnet pack -c Release -o bin
# Creates: bin/Release/Simple.HttpClientFactory.1.0.0.nupkg
```

### Publish to NuGet.org
```powershell
# 1. Get your API key from https://www.nuget.org/account/apikeys

# 2. Push package
$apiKey = "oy2a...YOUR_KEY...here"
$pkg = "bin/Release/Simple.HttpClientFactory.1.0.0.nupkg"

dotnet nuget push $pkg `
  --source https://api.nuget.org/v3/index.json `
  --api-key $apiKey `
  --skip-duplicate

# 3. Wait 10-30 minutes for NuGet.org to process
# 4. Check: https://www.nuget.org/packages/Simple.HttpClientFactory
```

### After Publishing
```powershell
# Tag in Git
git tag v1.0.0
git push origin v1.0.0

# Create GitHub Release on https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory/releases
# Add release notes from CHANGELOG.md
```

---

## 📋 Pre-Publishing Checklist

- [x] Version set to 1.0.0
- [x] All tests pass (7/7)
- [x] No compiler warnings
- [x] Package metadata complete
- [x] Icon included
- [x] License file present
- [x] README.md documented
- [x] CHANGELOG.md created
- [x] Release notes in package

**Status**: ✅ **READY TO PUBLISH**

---

## 📦 What's in Your Package

```
Simple.HttpClientFactory.1.0.0.nupkg
├── lib/net10.0/
│   ├── Simple.HttpClientFactory.dll      (compiled library)
│   └── Simple.HttpClientFactory.xml      (documentation)
├── icon.png                              (package icon)
├── README.md                             (from repo root)
└── [Content_Types].xml                   (NuGet metadata)

Simple.HttpClientFactory.1.0.0.snupkg      (symbols package)
└── src/                                   (embedded source)
```

---

## 🔑 Important Notes

⚠️ **API Key Safety**
- Never commit keys to Git
- Use GitHub Secrets for CI/CD: `${{ secrets.NUGET_API_KEY }}`
- Rotate keys periodically

⚠️ **Immutable Releases**
- Once published, a version can't be changed
- If you need to fix something, increment to 1.0.1
- Use `--skip-duplicate` to allow re-pushing same version (if needed)

⚠️ **Semantic Versioning**
- **1.0.0**: Initial stable
- **1.0.1**: Bug fix (patch)
- **1.1.0**: New features (minor)
- **2.0.0**: Breaking changes (major)

---

## 🚀 Verify After Publishing

1. ✅ Check package on: https://www.nuget.org/packages/Simple.HttpClientFactory
2. ✅ Icon displays correctly
3. ✅ Description renders properly
4. ✅ Tags are searchable
5. ✅ Can install: `dotnet add package Simple.HttpClientFactory`
6. ✅ IntelliSense works in consuming projects

---

## 📚 Full Guides

- **PUBLISHING_GUIDE.md** - Detailed step-by-step instructions
- **PUBLICATION_CHECKLIST.md** - Complete pre-publication checklist
- **PRE_PUBLICATION_SUMMARY.md** - What's been completed
- **CHANGELOG.md** - Release notes and features
- **README.md** - Package documentation

---

## 🎯 Package Details

| Property | Value |
|----------|-------|
| **Package ID** | Simple.HttpClientFactory |
| **Version** | 1.0.0 |
| **License** | MIT |
| **Author** | Dmitry Bychenko |
| **Repository** | https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory |
| **Target Framework** | .NET 10.0 |
| **Primary Dependency** | Microsoft.Extensions.Http 10.0.12 |
| **Tests** | 7 tests, all passing ✅ |

---

## ⚡ Common Commands

```powershell
# Build Release
dotnet build -c Release

# Run Tests
dotnet test

# Create Package
dotnet pack -c Release -o bin

# Push to NuGet
dotnet nuget push "bin/Release/Simple.HttpClientFactory.1.0.0.nupkg" `
  -s https://api.nuget.org/v3/index.json `
  -k your-api-key

# Install Locally (for testing)
dotnet add package Simple.HttpClientFactory `
  --version 1.0.0 `
  --source ./bin/Release

# List Local Packages
dotnet package search Simple.HttpClientFactory
```

---

## 🆘 Need Help?

**Troubleshooting**: See PUBLISHING_GUIDE.md → Troubleshooting section

**Quick Issues**:
- Invalid API key → Regenerate from https://www.nuget.org/account/apikeys
- Package not appearing → Wait 10-30 minutes then refresh
- Test failed → Run `dotnet test` to diagnose

---

**Your project is ready! Now go publish! 🚀**
