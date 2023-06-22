<h1 align="center">Simple API</h1>

---

### Build database

- To create and start mysql database with docker, run following command:

```shell
make mysql
```

- To create and start mongodb with docker, run below command:

```shell
make mongo
```

### Run

- To run simple API, run by follow command:

```shell
make run
```

- Or run with specified launch

```shell
make run launch={your-launch}
```

Now, you can go [here](http://localhost:5000/index.html) and try API

- For more information, you can run command:

```shell
make help
```
