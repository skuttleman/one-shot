# One Shot

A top-down stealth game for Unity3D.

## Dependencies

- Unity v2021.3.12f1
- AWS CLI tool

## Development

### Pulling Project

```bash
$ git clone https://github.com/skuttleman/one-shot.git
$ cd one-shot
$ aws s3 sync s3://one-shot Assets/resources
```

### Pushing Project

Make sure to push S3 assets as well.

```bash
$ git commit
$ git push
$ aws s3 sync Assets/resources s3://one-shot
```
