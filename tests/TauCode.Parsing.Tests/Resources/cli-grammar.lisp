; Migrate metadata (mm)
; e.g.: mm --conn="Server=.;Database=econera.diet.tracking;Trusted_Connection=True;" --provider=sqlserver --to=sqlite --target=c:/temp/mysqlite.json

(defblock :name mm :is-top t
	(exact-text :classes term :value "mm" :name command-mm)
	(idle :name args)
	(alt
		(seq
			(exact-text :classes key :value "conn" :name connection-key)
			(punctuation :value "=")
			(some-text :classes string :name connection-value)
		)
		(seq
			(exact-text :classes key :value "provider" :name provider-key)
			(punctuation :value "=")
			(some-text :classes term :name provider-value)
		)
		(seq
			(exact-text :classes key :value "to" :name to-key)
			(punctuation :value "=")
			(some-text :classes term :name to-value)
		)
		(seq
			(exact-text :classes key :value "target" :name target-key)
			(punctuation :value "=")
			(some-text :classes term key string path :name target-value)
		)
	)
	(idle :links args next)
	(end)
)