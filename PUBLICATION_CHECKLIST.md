# Pre-Publication Checklist for Plain.HttpClientFactory v1.0.0

## Code Quality ✅
- [x] All unit tests pass
- [x] No compiler warnings
- [x] Code style enforcement enabled
- [x] XML documentation complete
- [x] Nullable reference types enabled
- [x] Treat warnings as errors: enabled

## Documentation ✅
- [x] README.md complete with examples
- [x] CHANGELOG.md created
- [x] LICENSE file added (MIT)
- [x] Inline code documentation complete
- [x] API reference documented

## Package Metadata ✅
- [x] Version set to 1.0.0
- [x] Package ID: Plain.HttpClientFactory
- [x] Description: Clear and concise
- [x] Author: Dmitry Bychenko
- [x] License: MIT
- [x] Tags: Added multiple relevant tags
- [x] Package icon: icon.png included
- [x] Repository URL: GitHub
- [x] Release notes: Added to package metadata

## Source Link & Debugging ✅
- [x] PublishRepositoryUrl enabled
- [x] EmbedUntrackedSources enabled
- [x] DebugType: embedded
- [x] Microsoft.SourceLink.GitHub package added
- [x] Symbol package generation enabled (.snupkg)

## Pre-Publication Tasks

### Before Publishing
1. [ ] Run: `dotnet clean`
2. [ ] Run: `dotnet build -c Release`
3. [ ] Run: `dotnet test`
4. [ ] Generate package: `dotnet pack -c Release`
5. [ ] Inspect .nupkg file contents
6. [ ] Test installation in a new project: `nuget install Plain.HttpClientFactory -OutputDirectory .\packages`
7. [ ] Verify package on NuGet.org staging (if available)

### Git & Repository
1. [ ] Commit all changes: `git add . && git commit -m "Release v1.0.0"`
2. [ ] Create Git tag: `git tag -a v1.0.0 -m "Release version 1.0.0"`
3. [ ] Push changes: `git push origin master`
4. [ ] Push tags: `git push origin v1.0.0`

### NuGet Publishing
1. [ ] Create/verify NuGet.org account
2. [ ] Obtain API key from https://www.nuget.org/account/apikeys
3. [ ] Dry-run publish to verify package:
   ```powershell
   dotnet nuget push "bin/Release/Plain.HttpClientFactory.1.0.0.nupkg" \
     --source https://api.nuget.org/v3/index.json \
     --api-key YOUR_API_KEY \
     --skip-duplicate
   ```
4. [ ] Publish to NuGet: `dotnet nuget push "bin/Release/Plain.HttpClientFactory.1.0.0.nupkg" -s https://api.nuget.org/v3/index.json -k YOUR_API_KEY`
5. [ ] Monitor package processing on NuGet.org
6. [ ] Verify package appears in search results

### Post-Publication
1. [ ] Create GitHub Release from tag v1.0.0
2. [ ] Add release notes to GitHub Release
3. [ ] Announce on social media/dev communities
4. [ ] Update project documentation if needed
5. [ ] Monitor issues and feedback

## Quick Commands Reference

```powershell
# Clean and build release
dotnet clean
dotnet build -c Release

# Run tests
dotnet test

# Create package
dotnet pack -c Release

# List package contents
tar -tf bin/Release/Plain.HttpClientFactory.1.0.0.nupkg

# Push to NuGet
dotnet nuget push "bin/Release/Plain.HttpClientFactory.1.0.0.nupkg" `
  --source https://api.nuget.org/v3/index.json `
  --api-key [YOUR_KEY_HERE]

# Test install in new directory
mkdir test_install
cd test_install
dotnet add package Simple.HttpClientFactory
```

## Verification Checklist

After publishing, verify:
- [ ] Package appears on nuget.org
- [ ] Package icon displays correctly
- [ ] README renders properly
- [ ] License shows as MIT
- [ ] Tags are searchable
- [ ] Can install with `dotnet add package Plain.HttpClientFactory`
- [ ] IntelliSense works in consuming projects
- [ ] Symbol package (.snupkg) is available for debugging
- [ ] Source Link enables step-through debugging

## Support

For issues or questions about the publication process:
- NuGet Docs: https://docs.microsoft.com/en-us/nuget/
- Source Link Docs: https://github.com/dotnet/sourcelink
- Contact: Dmitry Bychenko on GitHub

---

**Status:** Ready for publication ✅  
**Date**: [Current Date]  
**Package**: Plain.HttpClientFactory v1.0.0
