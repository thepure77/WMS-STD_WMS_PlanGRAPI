1. �Դ command line cmd
2. ��� path 价�� Dockerfile ����
3. ���������ҧ image
	docker build -f Dockerfile -t plangrapi:v1 .
4. ���������ҧ container
	docker run --name PlanGRAPIContainer -p 8088:80 plangrapi:v1