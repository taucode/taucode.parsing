(defblock :name serialize-data :is-top t
	(worker
		:worker-name serialize-data
		:verbs "serialize-data" "sd"
		:doc "Serializes all tables' data."
		:usage-samples (
			"sd --conn Server=.;Database=my_db;Trusted_Connection=True; --provider sqlserver --file c:/temp/my.json"
			"serialize-data -c Server=some-host;Database=my_db; -p postgresql -f c:/work/another.json"
			))
	(idle :name args)
	(alt
		(seq
			(multi-text
				:classes key
				:values "-c" "--connection"
				:alias connection
				:action key)
			(some-text
				:classes path
				:action value)
		)
		(seq
			(multi-text
				:classes key
				:values "-p" "--provider"
				:alias provider
				:action key)
			(multi-text
				:classes term
				:values "sqlserver" "postgresql"
				:action value)
		)
		(seq
			(multi-text
				:classes key
				:values "-f" "--file"
				:alias file
				:action key)
			(some-text
				:classes path
				:action value)
		)
		(fallback :name bad-key-fallback)
	)
	(idle :links args next)
	(end)
)
