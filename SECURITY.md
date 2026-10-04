# Security Policy

## Overview

Simple.HttpClientFactory takes security seriously. This document describes how to report security vulnerabilities and our security support policy.

## Reporting Security Vulnerabilities

**Please do not open a public GitHub issue for security vulnerabilities.**

Instead, please report security issues responsibly using GitHub's **Private Vulnerability Reporting** feature or by contacting the maintainers directly.

### Private Vulnerability Reporting

GitHub provides a secure way to report vulnerabilities:

1. Visit the [Security tab](https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory/security) of this repository
2. Click **"Report a vulnerability"**
3. Fill out the vulnerability details form
4. Submit your report

### Alternative Reporting

If you cannot use GitHub's private reporting, you can contact the maintainers through:
- **Email**: Please create a security advisory issue on GitHub for confidential contact information
- **GitHub Security Advisory**: Use the dedicated security advisory feature

### Information to Include

When reporting a vulnerability, please provide:

```
- Description of the vulnerability
- Affected version(s)
- Steps to reproduce (if applicable)
- Potential impact
- Suggested fix (if you have one)
- Your contact information (email preferred)
```

**Example:**
```
Vulnerability: Improper input validation in HttpClientFactory
Affected Version: 1.0.0
Description: The factory does not validate... [detailed description]
Impact: Could lead to... [potential consequences]
Steps to Reproduce:
1. Create a factory with...
2. Call method with...
Suggested Fix: Validate input before...
```

## Response Process

We aim to:

1. **Acknowledge receipt** of your report within 24-48 hours
2. **Assess and confirm** the vulnerability within 5-7 business days
3. **Develop a fix** as quickly as the severity dictates
4. **Release a patch** with the fix in a new version
5. **Coordinate disclosure** with you before public announcement
6. **Recognize your contribution** (with your permission)

### Response Timeline by Severity

| Severity | Timeline | Example |
|----------|----------|---------|
| **Critical** | Immediate | Remote code execution, data breach |
| **High** | 24-48 hours | Authentication bypass, privilege escalation |
| **Medium** | 1-2 weeks | Information disclosure, denial of service |
| **Low** | 1-4 weeks | Minor issues, edge cases |

## Version Support Policy

### Supported Versions

| Version | Status | Security Updates | End of Life |
|---------|--------|------------------|-------------|
| 1.0.x | Current | ✅ Yes | TBD |
| < 1.0.0 | Pre-release | ❌ No (beta only) | N/A |

- **Latest version**: Receives all security updates
- **Previous major versions**: May receive critical security patches at maintainer discretion
- **Pre-release versions**: No security support (use at your own risk)

### Reporting Security Issues in Older Versions

If you discover a vulnerability in an older version:
1. Please report it anyway (even if not supported)
2. We'll assess if it affects the current version
3. We may backport fixes if the vulnerability is critical

## Security Best Practices for Users

### When Using Simple.HttpClientFactory

1. **Keep Dependencies Updated**
   ```powershell
   dotnet add package Simple.HttpClientFactory --upgrade
   dotnet add package Microsoft.Extensions.Http --upgrade
   ```

2. **Monitor Security Advisories**
   - Watch this repository for security updates
   - Subscribe to GitHub security notifications
   - Follow .NET security feeds

3. **Input Validation**
   - Always validate URLs before passing to HttpClient
   - Sanitize headers and cookies from untrusted sources
   - Validate response content before using

4. **Sensitive Data**
   - Don't hardcode credentials in your code
   - Use secure credential storage (Environment variables, Azure Key Vault, etc.)
   - Be careful with logging HTTP requests/responses containing sensitive data

5. **Certificate Validation**
   - Ensure SSL/TLS certification validation is enabled
   - Don't disable certificate pinning in production
   - Use HttpClient with proper handler configuration

### Example Secure Usage

```csharp
using Simple.HttpClientFactory;

// ✅ SECURE: Use factory with proper configuration
var factory = new DefaultHttpClientFactory();
var httpClient = factory.CreateClient("secure-client");

// ✅ SECURE: Store credentials safely
var apiKey = Environment.GetEnvironmentVariable("API_KEY");
httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

// ✅ SECURE: Use HTTPS only
var response = await httpClient.GetAsync("https://api.example.com/data");

// ✅ SECURE: Validate response
if (response.IsSuccessStatusCode) {
    var content = await response.Content.ReadAsStringAsync();
    // Process content safely
}
```

## Dependencies and Supply Chain Security

### Direct Dependencies

- **Microsoft.Extensions.Http** (10.0.12)
  - Track: [Microsoft Security Updates](https://github.com/dotnet/runtime/security)
  - Part of .NET ecosystem

### Build Dependencies

- **xUnit** (testing)
- **WireMock.Net** (testing)
- **Microsoft.NET.Test.Sdk** (testing)

All dependencies are regularly monitored for security issues.

### Dependency Updates

- We monitor NuGet advisories
- Critical vulnerabilities trigger immediate patches
- Regular updates in maintenance releases
- Dependabot alerts enable early detection

## Security Headers and Configuration

### What This Package Does

✅ **Provides:**
- Shared `SocketsHttpHandler` with automatic decompression
- Thread-safe connection pooling
- Cookie container support
- Default credentials handling

### What This Package Does NOT Do (Users' Responsibility)

❌ **NOT Included:**
- SSL/TLS certificate pinning
- Request signing or HMAC authentication
- Rate limiting or throttling
- Advanced middleware chains
- Request/response encryption

These are typically handled at the application level or with specialized libraries.

## Known Vulnerabilities

Currently, no known security vulnerabilities.

To check for vulnerabilities in dependencies:
```powershell
dotnet list package --vulnerable
```

## Security Testing

This project includes:

- ✅ Unit tests with mocked HTTP servers (WireMock.Net)
- ✅ No actual network calls in tests (prevents SSRF risks)
- ✅ Static code analysis
- ✅ Compiler warnings treated as errors
- ✅ Nullable reference types enabled (null safety)

## Compliance & Standards

### .NET Security Guidelines

This project follows Microsoft's recommendations:
- [Managed security threats](https://docs.microsoft.com/en-us/dotnet/standard/security/)
- [Secure coding guidelines](https://docs.microsoft.com/en-us/dotnet/standard/security/secure-coding-guidelines)
- [OWASP guidelines](https://owasp.org/)

### Code Quality

- Enforced code style in build
- Compiler warnings as errors
- Nullable reference types enabled
- XML documentation
- 100% test coverage on business logic

## Vulnerability Disclosure Policy

### Coordinated Disclosure

We follow a coordinated disclosure model:

1. **Reporter submits vulnerability privately**
2. **We acknowledge and assess**
3. **We develop and test a fix**
4. **We release a patch version**
5. **After release, we disclose publicly**, including:
   - Details of the vulnerability
   - Who reported it
   - CVE (if applicable)
   - Credit to the reporter (with permission)

### Public Disclosure Timeline

- **Minimum 30 days** between patch release and detailed disclosure
- Allows users time to upgrade
- Respects embargo agreements if applicable

## Acknowledgments

We're grateful to security researchers who responsibly report vulnerabilities. We will:

- ✅ Acknowledge your report
- ✅ Credit you in release notes (if desired)
- ✅ Provide CVE details (if applicable)
- ✅ Thank you publicly (with permission)

## CVE (Common Vulnerabilities and Exposures)

For any CVEs related to this project:

- Check [NVD Database](https://nvd.nist.gov/)
- Search: "Simple.HttpClientFactory"
- Subscribe to [CVE Alerts](https://nvd.nist.gov/api)

## Security Contacts

- **Primary Maintainer**: Dmitry Bychenko
- **Repository**: https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory
- **Issues**: https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory/issues

## Additional Resources

### .NET Security Documentation
- [.NET Security Overview](https://docs.microsoft.com/en-us/dotnet/standard/security/)
- [HttpClient Security](https://docs.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory)
- [Secure Coding Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/security/secure-coding-guidelines)

### OWASP Resources
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [API Security](https://owasp.org/www-project-api-security/)

### Related GitHub Security
- [GitHub Security Lab](https://securitylab.github.com/)
- [GitHub Advisories](https://github.com/advisories)
- [Dependabot](https://dependabot.com/)

## Changes to This Policy

We may update this security policy as needed. Changes will be:
- Posted to this file
- Committed to the repository
- Announced in release notes (if significant)

**Last updated**: [Current Date]  
**Version**: 1.0

---

**Thank you for helping keep Simple.HttpClientFactory secure!** 🔒

If you have any questions about this policy, please open a regular GitHub issue (not for reporting vulnerabilities).
