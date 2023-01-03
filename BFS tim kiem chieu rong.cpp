#include<bits/stdc++.h>

#define REP(i , l , r) for(int i = l ; i <= r ; i++)
#define REPD(i , l , r) for(int i = l ; i >= r ; i--)
#define REPS(i , l , r) for(int i = l ; i < r ; i++)

typedef long long int ll;

using namespace std;

enum facing {
	LEFT , RIGHT , UP , DOWN , STAY //LEFT=0 , RIGHT=1 , UP=2 , DOWN=3 , STAY=4
};

int puzzle[3][3] , posX , posY , checker;
// khoi tao cac node nhu la cac trang thai cua puzzle
class node{
	public :
		int arr[3][3] , x , y;
		string way;//dung de dem so buoc di
		facing canFace;//cac buoc di co the
		
		node(int a[3][3] , string way , facing canFace , int x , int y){
			this->way = way;
			this->canFace = canFace;
			this->x = x;// x la so hang (height)
			this->y = y;// y la so cot (width)
			REP(i,0,2){ // cac trang thai ban dau
				REP(j,0,2){
					arr[i][j] = a[i][j];
				}
			}
		}
		
		bool canMoveLeft(){
			return canFace != LEFT && y > 0;//buoc di khong phai left va chieu rong 1 hoac 2 thi co the sang trai
		}
		bool canMoveRight(){
			return canFace != RIGHT && y < 2;//tuong tu 
		}
		bool canMoveUp(){
			return canFace != UP && x > 0;//tuong tu
		}
		bool canMoveDown(){
			return canFace != DOWN && x < 2;//tuong tu
		}
		
		void moveLeft(){
			swap(arr[x][y] , arr[x][y-1]);//doi cho 2 vi tri voi nhau
			y--;// luu vi tri y moi
			canFace = RIGHT;//co the qua phai?
			way += "l";//them vao way de tinh tong
		}
		void moveRight(){
			swap(arr[x][y] , arr[x][y+1]);
			y++;
			canFace = LEFT;
			way += "r";//tuong tu
		}
		void moveUp(){
			swap(arr[x][y] , arr[x-1][y]);
			x--;
			canFace = DOWN;
			way += "u";//tuong tu
		}
		void moveDown(){
			swap(arr[x][y] , arr[x+1][y]);
			x++;
			canFace = UP;
			way += "d";//tuong tu
		}
		
		bool checkFinish(){
			if(checker == 1){
				REP(i,0,2){
					if(arr[0][i] != i+1 || arr[2][i] != 7-i) return false;;//neu cac o cua no khac gia tri finish thi false 
				}//vi du arr[0][0] = 1 thi moi dung cho truong hop A voi N le
				return arr[1][0] != 8 || arr[1][2] != 4 ? false : true;//xet them o bang 8 và 4 vì trên thieu, neu khac thi false, neu dung thi true
			}else{
				REP(i,0,2){
					if(arr[0][i] != i || arr[1][i] != i+3 || arr[2][i] != i+6) return false;//
				}//tuong tu cho trng thai B voi N chan
				return true;
			}
			
		}
};
//thuc hien hoan doi gia tri o vi tri hien tai voi vi tri lien ke sau do cap nhat lai
void moveLeft(){
	swap(puzzle[posX][posY] , puzzle[posX][posY-1]);
	posY--;//giam posy de cap nhat vi tri hien tai
}
void moveRight(){
	swap(puzzle[posX][posY] , puzzle[posX][posY+1]);
	posY++;//tuong tu
}
void moveUp(){
	swap(puzzle[posX][posY] , puzzle[posX-1][posY]);
	posX--;//tuong tu
}
void moveDown(){
	swap(puzzle[posX][posY] , puzzle[posX+1][posY]);
	posX++;//tuong tu
}
// ham nhap du lieu
void initPuzzle(){
	cout << "Nhap cac gia tri cho puzzle : ";
	cin >> puzzle[0][0] >> puzzle[0][1] >> puzzle[0][2];
	cin >> puzzle[1][0] >> puzzle[1][1] >> puzzle[1][2];
	cin >> puzzle[2][0] >> puzzle[2][1] >> puzzle[2][2];
	
	bool checked = true;//gia su bang true
	int sum = 0;
	REP(i,0,2){
		REP(j,0,2){
			sum += puzzle[i][j];//tinh tong cac phan tu trong mang
			if(puzzle[i][j] > 8) {//1 phan tu >8 thi false
				checked = false;
				break;
			}
		}
	}
	if(sum != 36 || checked == false){//tong phai bang 36
		cout << "Nhap sai du lieu vui long nhap lai" << endl;
		return initPuzzle();
	}
	
	REP(i,0,2){
		REP(j,0,2){
			if(puzzle[i][j] == 0){//=0 là ô mau den và gán i j cho nó
				posX = i;
				posY = j;
				return;
			}
		}
	}
}
// ham in trang thai
void prin(){//duyêt tung phân tu và in ra nó
	REP(i,0,2){
		REP(j,0,2){
			cout << puzzle[i][j] << " ";
		}
		cout << endl;
	}
}
// ham kiem tra xem da la trang thai dich chua		
bool checkFinish(){
	int counter1 = 0, counter2 = 0;//bien dem so luong ô sap xep dung
	REP(i,0,2){//truong hop cho A
		if(puzzle[0][i] == i+1) counter1++;
	}
	
	REP(i,0,2){
		if(puzzle[2][i] == 7-i) counter1++;
	}
	if(puzzle[1][0] == 8){
		counter1++;
	} 
	if(puzzle[1][2] == 4) {
		counter1++;
	}
	if(counter1 == 8) return true;//neu ca 8 o deu dung thi true
		
	REP(i,0,2){//truong hop cho B
		if(puzzle[0][i] == i) counter2++;
		if(puzzle[1][i] == i+3) counter2++;
		if(puzzle[2][i] == i+6) counter2++;
	}
	if(counter2 == 8) return true;
	return false;
}
// ham xac dinh trang thai dich
int countStart(){
	int sum = 0;
	REP(q,0,8){//vong lap ngoai tu 0 den 8
		int row = q/3;
		/* Vi du khi q la 0 row se la 0 col se la 0 va counter se la puzzle [0] [0] 
		Khi q la 1 row se la 0 col se la 1 va counter se la puzzle [0] [1] Van van cho
		 den khi q la 8 row se la 2 col se la 2 va counter se la puzzle [2] [2]*/
		int col = q%3;
		int counter = puzzle[row][col];//luu vao bien counter
		REP(i,0,2){//vong lap trong lap qua trung phan tu cua puzzle
			REP(j,0,2){
				if( (row < i && puzzle[i][j] < counter && puzzle[i][j] != 0 )  ){
					sum++;//neu hang vi tri hien tai nho hon i va gia tri puzzle nho hon gia tri hien tai va khac 0 thi tang sum
				}else if(row == i && col < j && puzzle[i][j] < counter && puzzle[i][j] != 0){
					sum++;//tuong tu neu hang bang i thi xet j
				}//tinh toan so luong phan tu nho hon gia tri tai vi tri hien tai counter va khac 0 trong cac hang va cot truoc vi tri hien tai
			}
		}
	}
	return sum;//in ra N la chan hoac le de xac dinh trang thai dich
}

int main(){
	//int step = 0 ;
	ll numOfNode = 0;//so node
	bool check = checkFinish();//check la tt cuoi chua(true/false)
	
	initPuzzle();//ham nhap du lieu
	const clock_t begin_time = clock();
	string way = "";//string way rong de dem buoc di
	node nd(puzzle , "" , STAY , posX , posY);//tao node dung yen
	vector<node> vt; // tao hang` doi
	vt.push_back(nd);//thêm phantu vào node
	
	checker = countStart() %2;//dung de check xem N chan hay le de xac dinh trang thai dich
	cout << "Trang thai ban dau : " << endl;
	prin();//ham in trang thai dau 
	cout << endl;
	while(!check){//tim kiem BFS
		vector<node> open;//node open duyet cac node cua BFS
		REPS(i,0,vt.size()){ //duyet theo cac ptu co trong vt
			numOfNode++;//tang so node dc duyet
			if(vt.at(i).checkFinish()){//kiem tra co phai node ket thuc ko
				way = vt.at(i).way;//neu ket thuc thi way bang gia tri cua way
				check = true;//
				break;//thoat khoi vong
			}else{
				if(vt.at(i).canMoveLeft()){//kiem tra node thu i co the sang trai hay ko 
					node nd = vt.at(i);//neu co the thi tao node moi va sao chep node i vao 
					nd.moveLeft();//goi moveleft de chuyen node sang trai
					open.push_back(nd);//sau do them node vao cuoi vecto open
				}
				if(vt.at(i).canMoveRight()){
					node nd = vt.at(i);
					nd.moveRight();
					open.push_back(nd);
				}
				if(vt.at(i).canMoveUp()){
					node nd = vt.at(i);
					nd.moveUp();
					open.push_back(nd);
				}
				if(vt.at(i).canMoveDown()){
					node nd = vt.at(i);
					nd.moveDown();
					open.push_back(nd);
				}
			}
			
		}
		vt.clear();//xóa tat ca phan tu trong vt
		REPS(i,0,open.size()){//duyet qua open
			vt.push_back(open.at(i));//them phan tu i trong open vao vt
		}
	}
	
	REPS(i,0,way.length()){//thuc hien cac buoc di da duyet
		if(way[i] == 'l'){
			moveLeft();//thuc hien di sang trai
			prin();
			cout << "di chuyen sang trai" << endl << endl;
		}else if(way[i] == 'r'){
			moveRight();
			prin();
			cout << "di chuyen sang phai" << endl << endl;
		}else if(way[i] == 'u'){
			moveUp();
			prin();
			cout << "di chuyen len tren" << endl << endl;
		}else if(way[i] == 'd'){
			moveDown();
			prin();
			cout << "di chuyen xuong duoi" << endl << endl;
		}
	}//hien thi thong tin
	cout << "Thuat toan BFS : " << endl;
	cout << "So buoc di = " << way.length() << endl;
	cout << "So phep toan da thuc hien = " << numOfNode << endl;
	cout << "Thoi gian tinh toan = " << float( clock () - begin_time ) /  CLOCKS_PER_SEC << "s";
	return 0;
}
