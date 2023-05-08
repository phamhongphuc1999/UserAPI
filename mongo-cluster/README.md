<h1 align="center">MongoDB (6.0.1) Sharded Cluster with Docker Compose</h1>

### How to setup and examine your mongo cluster?

All config is inspired in [here](https://github.com/minhhungit/mongodb-cluster-docker-compose)

#### Step 1: Start all the containers

```shell
docker-compose up -d
```

#### Step 2: Initialize the replica sets (config servers and shards)

- Config servers

```shell
docker-compose exec configsvr01 sh -c "mongosh < /scripts/init-configserver.js"
```

- config shards

```shell
docker-compose exec shard01-a sh -c "mongosh < /scripts/init-shard01.js"
docker-compose exec shard02-a sh -c "mongosh < /scripts/init-shard02.js"
docker-compose exec shard03-a sh -c "mongosh < /scripts/init-shard03.js"
```

#### Step 3: Initializing the router

```shell
docker-compose exec router01 sh -c "mongosh < /scripts/init-router.js"
```

#### Step 4: Enable sharding and setup sharding-key

- Connect router to port

```shell
docker-compose exec router01 mongosh --port 27017
```

- Enable sharding for database `MyDatabase`

```shell
sh.enableSharding("MyDatabase")
```

- Setup shardingKey for collection `MyCollection`

```shell
db.adminCommand( { shardCollection: "MyDatabase.MyCollection", key: { oemNumber: "hashed", zipCode: 1, supplierId: 1 } } )
```

#### Done!

- Btw, here is mongodb connection string if you want to try to connect mongodb cluster with MongoDB Compass from your
  host computer (which is running docker)

```shell
mongodb://127.0.0.1:27117,127.0.0.1:27118
```

### Reference

- [https://coinmarketcap.com/api/documentation/v1/#](https://coinmarketcap.com/api/documentation/v1/#)
